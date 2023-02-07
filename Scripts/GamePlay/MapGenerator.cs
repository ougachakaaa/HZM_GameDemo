using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    //alway update when parameters change;
    [HideInInspector][SerializeField]
    private bool _alwayUpdate;

    public Transform mapHolder;

    //prefabs
    public Transform tilePrefab;
    public Transform navmeshFloor;
    public Transform navmeshMaskPrefab;
    public Transform obstaclePrefab;

    //tiles
    [Range(0,1)] 
    public float outlineSize;
    public Transform[,] tiles;
    List<Coord> allTileCoords;

    //size
    public float mapUnitSize;
    public Vector2 maxMapSize;

    //map
    public Map currentMap;
    public List<Map> maps = new List<Map>();
    public System.Random prng;
    public int obstacleHeightStep;
    private void OnEnable()
    {
        //Actions.OnEnemySpawn += currentMap.CoordToPosition;
    }
    private void OnDisable()
    {
        //Actions.OnEnemySpawn -= currentMap.CoordToPosition;
    }
    public void Start()
    {
        GenerateMap(0);
    }

    //map
    public void GenerateMap(int index)
    {
        // test

        // test
        currentMap = maps[index];
        navmeshFloor.localScale = new Vector3(maxMapSize.x, maxMapSize.y,0) * mapUnitSize;

        currentMap.obstacleMap = new bool[currentMap.mapSize.x, currentMap.mapSize.y];

        GenerateTileCoords();
        GenerateTiles();

        ShuffleTileCoords();
        GenerateObstacle();

        GenerateNavmeshMask();

    }
    void GenerateNavmeshMask()
    {
        Debug.Log("Generate NavmeshMask");
        Transform holder = GenerateHolder("NavmeshMaskHolder");

        float xDistanceToCenter = (float)(maxMapSize.x + currentMap.mapSize.x) / 4 * mapUnitSize;
        float zDistanceToCenter = (float)(maxMapSize.y + currentMap.mapSize.y) / 4 * mapUnitSize;
        float xDif = (float)(maxMapSize.x - currentMap.mapSize.x) / 2 * mapUnitSize;
        float zDif = (float)(maxMapSize.y - currentMap.mapSize.y) / 2 * mapUnitSize;

        Vector3[] maskPositions = {
            new Vector3(xDistanceToCenter,mapUnitSize/2,0) ,    //0 right 
            new Vector3(-xDistanceToCenter,mapUnitSize/2,0) ,   //1 left
            new Vector3(0,mapUnitSize/2,zDistanceToCenter) ,    //2 up
            new Vector3(0,mapUnitSize/2,-zDistanceToCenter) ,   //3 down
            };

        Transform[] NavmeshMasks = new Transform[4];
        for (int i = 0; i < NavmeshMasks.Length; i++)
        {
            NavmeshMasks[i] = Instantiate(navmeshMaskPrefab, maskPositions[i], Quaternion.identity, holder);

            if (i<2)
                NavmeshMasks[i].localScale = new Vector3(xDif, mapUnitSize, maxMapSize.y*mapUnitSize);
            else
                NavmeshMasks[i].localScale = new Vector3(maxMapSize.x*mapUnitSize, mapUnitSize, zDif);
        }


    }

    //tile
    void GenerateTileCoords()
    {
        allTileCoords = new List<Coord>();
        for (int x = 0; x < currentMap.mapSize.x; x++)
        {
            for (int y = 0; y < currentMap.mapSize.y; y++)
            {
                allTileCoords.Add(new Coord(x, y));
            }
        }
    }
    void GenerateTiles()
    {
        tilePrefab.transform.localScale = mapUnitSize * Vector3.one;

        Transform tilesHolder = GenerateHolder("TilesHolder");

        tiles = new Transform[currentMap.mapSize.x, currentMap.mapSize.y];
        for (int x = 0; x < currentMap.mapSize.x; x++)
        {
            for (int y = 0; y < currentMap.mapSize.y; y++)
            {
                //generate coords
                Coord thisCoord = new Coord(x, y);
                Vector3 tilePos = currentMap.CoordToPosition(thisCoord) * mapUnitSize;

                tiles[x, y] = Instantiate<Transform>(tilePrefab, tilePos, Quaternion.Euler(Vector3.right * 90), tilesHolder);
                tiles[x, y].name = $"Tile:({x},{y})";
                tiles[x, y].localScale = (1 - outlineSize) * tiles[x, y].localScale;

            }
        }
    }

    //obstacle
    void GenerateObstacle()
    {
        Debug.Log("Generate Obstacle");
        Transform obstacleHolder = GenerateHolder("ObstcleHolder");

        prng = new System.Random(currentMap.obstacleSeed);

        int obstacleCount = Mathf.RoundToInt(currentMap.mapSize.x * currentMap.mapSize.y * currentMap.obstaclePercentage);
        int currentObstacleCount = 0;


        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = currentMap.GetRandomCoord();
            currentObstacleCount++;
            currentMap.obstacleMap[randomCoord.x, randomCoord.y] = true;
            if (randomCoord != currentMap.mapCenterCoord && IsMapFullyAccessible(currentMap.obstacleMap,currentObstacleCount))
            {
                float obstacleHeight = Mathf.Lerp(
                    currentMap.minObstacleHeight, 
                    currentMap.maxObstacleHeight, 
                    CalculateObstacleHeight((float)prng.NextDouble()));
                Transform newObstacle = Instantiate<Transform>(
                    obstaclePrefab, 
                    (currentMap.CoordToPosition(randomCoord) +Vector3.up*obstacleHeight/2)*mapUnitSize, 
                    Quaternion.identity, 
                    obstacleHolder);

                newObstacle.transform.localScale =
                    new Vector3(1, obstacleHeight,1) * mapUnitSize;

                Renderer obstacleRenderer = newObstacle.GetComponentInChildren<Renderer>();
                Material obstacleMaterial = new Material(obstacleRenderer.sharedMaterial);
                float colorPercent = (float)randomCoord.y / currentMap.mapSize.y;
                obstacleMaterial.color = Color.Lerp
                    (currentMap.foreGroundColor, currentMap.backGroundColor, colorPercent);

                obstacleRenderer.sharedMaterial = obstacleMaterial;
            }
            else
            {
                currentObstacleCount--;
                currentMap.obstacleMap[randomCoord.x, randomCoord.y] = false;
            }
        }
    }

    void ShuffleTileCoords()
    {
        currentMap.shuffleTileCoords = new Queue<Coord>(Utility.ShuffleArray<Coord>(allTileCoords.ToArray(), currentMap.obstacleSeed));
    }

    Transform GenerateHolder(string holderName)
    {
        if (mapHolder.Find(holderName))
        {
            DestroyImmediate(mapHolder.Find(holderName).gameObject);
        }
        Transform holder = new GameObject(holderName).transform;
        holder.parent = mapHolder;
        return holder;
    }
    bool IsMapFullyAccessible(bool[,] _obstacleMap,int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[_obstacleMap.GetLength(0),_obstacleMap.GetLength(1)];
        Queue<Coord> coordsToCheck = new Queue<Coord>();
        coordsToCheck.Enqueue(currentMap.mapCenterCoord);
        mapFlags[currentMap.mapCenterCoord.x, currentMap.mapCenterCoord.y] = true;

        int accessibleTileCount = 1;

        while (coordsToCheck.Count > 0)
        {
            Coord thisCoord = coordsToCheck.Dequeue();
            for (int x = -1; x <=1 ; x++)
            {
                for (int y = -1; y <=1 ; y++)
                {
                    int neighbourX = thisCoord.x + x;
                    int neighbourY = thisCoord.y + y;
                    //exclude diagonal tile
                    if (x ==0||y==0)
                    {
                        if (neighbourX >= 0 && neighbourX < _obstacleMap.GetLength(0) && 
                            neighbourY >= 0 && neighbourY < _obstacleMap.GetLength(1))
                        {
                            if (!mapFlags[neighbourX,neighbourY] && !_obstacleMap[neighbourX,neighbourY])
                            {
                                mapFlags[neighbourX, neighbourY] = true;
                                coordsToCheck.Enqueue(new Coord(neighbourX,neighbourY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }
        return (mapFlags.Length - accessibleTileCount == currentObstacleCount);
    }

    float CalculateObstacleHeight(float height)
    {
        if (obstacleHeightStep<1)
        {
            height = currentMap.minObstacleHeight;
        }
        else if (obstacleHeightStep < 2)
        {
            height = currentMap.maxObstacleHeight;
        }
        else
        {
            for (int i = 0; i < obstacleHeightStep; i++)
            {
                if (height >= i * 1f / obstacleHeightStep && height <= (i+1)* 1f / obstacleHeightStep)
                {
                    height = (i+1) * 1f / obstacleHeightStep;
                    break;
                }
            }
        }
        return height;
    }

    public IEnumerator TileEffect(Coord tileCoord,float duration =1f)
    {
        float timer = 0f;
        Material tileMat = tiles[tileCoord.x, tileCoord.y].GetComponent<Renderer>().material;
        Color initialColor = tileMat.color;
        Color flashColor = Color.red;
        int tileFlashCount = 3;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            tileMat.color = Color.Lerp(initialColor, flashColor, Mathf.PingPong(timer * tileFlashCount, 1));
            yield return null;
        }
        tileMat.color = initialColor;
        yield return null;
    }
}
public struct Coord
{
    public int x;
    public int y;
    public Coord(int _x,int _y)
    {
        x = _x;
        y = _y;
    }

    public static bool operator ==(Coord c1, Coord c2)
    {
        return (c1.x == c2.x && c1.y == c2.y);
    }
    public static bool operator !=(Coord c1, Coord c2)
    {
        return !(c1 == c2);
    }

}

[System.Serializable]
public class Map
{
    //map size
    public Vector2Int mapSize;
    public Coord mapCenterCoord {
        get => new Coord(mapSize.x / 2, mapSize.y / 2);
    }
    public Vector3 mapCenterPos
    {
        get => new Vector3(mapSize.x / 2f,0f, mapSize.y / 2f);
    }

    //obstacles
    [Range(0, 1)]
    public float obstaclePercentage;
    public int obstacleSeed;
    public Queue<Coord> shuffleTileCoords;
    public float minObstacleHeight;
    public float maxObstacleHeight;
    public Color foreGroundColor;
    public Color backGroundColor;
    public bool[,] obstacleMap;

    //open tile
    //public Color 

    public Coord GetRandomOpenTileCoord()
    {
        Coord coord = mapCenterCoord;
        for (int i = 0; i < shuffleTileCoords.Count; i++)
        {
            coord = GetRandomCoord();
            if (obstacleMap[coord.x,coord.y] == false)
            {
                return coord;
            }
        }
        Debug.Log("no open tile found?");
        return coord;
    }
    public Coord GetRandomCoord()
    {
        Coord randomCoord = shuffleTileCoords.Dequeue();
        shuffleTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    //position & coord transition
    public Vector3 CoordToPosition(Coord coord)
    {
        return new Vector3(0.5f + coord.x, 0, 0.5f + coord.y)- mapCenterPos;
    }
    public Coord PositionToCoord(Vector3 position)
    {
        Vector3 pos = position + mapCenterPos;
        int x = Mathf.RoundToInt(pos.x -0.5f);
        int y = Mathf.RoundToInt(pos.y -0.5f);
        return new Coord(x,y);
    }
}
