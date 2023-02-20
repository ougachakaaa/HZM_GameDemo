using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    Survive,
    DiamondCollect,
    Demo,
}
public class MapManager : MonoBehaviour
{
    //levels
    public  LevelParamHolder levelParam;
    public int mapIndex;
    public GameMode gameMode;

    //alway update when parameters change;
    [HideInInspector][SerializeField]
    private bool _alwayUpdate;
    public static readonly byte voxelOccupied = byte.MaxValue;
    System.Random prng;

    [Header("Map prefab")]
    //prefabs and prefab holders
    public Transform mapHolder;
    Transform boundaryHolder;
    Transform navmeshFloor;
    Transform deathVolume;
    public Transform mapBoundaryPrefab;
    float boundaryHeight;

    //voxel prefabs
    [Header("Voxel Behavior")]
    public List<VoxelBehavior> voxelBehaviors;
    int behaviorCount;
    public Transform[] voxelBehaviorHolders;

    //maps
    [Header("Maps")]
    public float mapUnitSize;
    public Vector2 maxMapSize;
    public Map currentMap;
    public List<Map> maps;
    public Transform[,,] voxelTransforms;

    //tiles
    [Header("Tiles")]
    [Range(0,1)] 
    public float outlineSize;
    List<Coord> allTileCoords;


    public void Start()
    {
        LoadLevelParamter();
        InitializeMapManager(mapIndex);
        GenerateMap();
        switch (gameMode)
        {
            case GameMode.Survive:
                break;
            case GameMode.DiamondCollect:
                GenerateMaze();
                break;
            case GameMode.Demo:
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        if (!Actions.IsPlayerDead && voxelBehaviors.Count>0 && gameMode !=GameMode.DiamondCollect)
        {
            foreach (VoxelBehavior behavior in voxelBehaviors)
            {
                if (behavior.executor == VoxelBehavior.Executor.MapManager)
                {
                    ExecutingVoxelBehaivor(behavior);
                }
            }
        }
    }

    void LoadLevelParamter()
    {
        levelParam = LevelParamHolder.Instance;
        mapIndex = levelParam.levelIndex;
        gameMode = levelParam.mode;
        boundaryHeight = 20;
        if (gameMode == GameMode.Demo)
        {
            maxMapSize = new Vector2Int(9, 9);
            boundaryHeight = 2f;
            mapIndex = maps.Count-1;
        }
    }

    public void InitializeMapManager(int index)
    {
        mapHolder = GameObject.Find("MapHolder").transform;
        navmeshFloor = mapHolder.Find("NavmeshFloor");
        deathVolume = mapHolder.Find("DeathVolume");
        behaviorCount = voxelBehaviors.Count;

        currentMap = maps[index];
        currentMap.InitializeMap();
        prng = new System.Random(currentMap.obstacleSeed);

        navmeshFloor.localScale = new Vector3(maxMapSize.x, maxMapSize.y, 0) * mapUnitSize;
        voxelBehaviorHolders = new Transform[behaviorCount];
        voxelTransforms = new Transform[currentMap.mapSize.x, currentMap.mapSize.y, behaviorCount];

        for (int i = 0; i < behaviorCount; i++)
        {
            voxelBehaviors[i].InitializeBehavior(currentMap);
            string holderName = $"{voxelBehaviors[i].name}Holder";
            voxelBehaviorHolders[i] = GenerateHolder(holderName);
        }
    }
    public void GenerateMap()
    {
        GenerateMapCoords();
        GenerateMapBoundary();

        //initialize current map with tils
        foreach (Coord coord in allTileCoords)
        {
            GenerateVoxel(coord, (byte)VoxelBehaviorType.OpenTile);
        }
    }
    void GenerateMapCoords()
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
    
    
    void ExecutingVoxelBehaivor(VoxelBehavior b)
    {
        if (b.isEnabled)
        {
            if (b.CheckCount()  && b.hasAvailableTile && b.CheckTimer())
            {
                Debug.Log("VB:" + b.name);
                Coord voxelCoord = currentMap.GetRandomCoordOfType(b.OriginalType);
                StartCoroutine(PlayVoxelBehaviorEffect(voxelCoord,b,TurnVoxelToTargetType));
                b.IncreaseCount(1);
                b.TargetTypeCountCheck();
            }
/*            //revert check
            if (b.isLooping && (!b.CheckCount() || !b.hasAvailableOpenTile ) && b.CheckTimer())
            {
                Coord voxelCoord = currentMap.GetRandomCoordOfType(b.TargetTypeIndex);
                StartCoroutine(FlashEffect(voxelCoord, b, TurnVoxelToTargetType));
                b.IncreaseCount(-1);
                b.MapOpentTileCountCheck();
            }*/
        }
    }


    void TurnVoxelToTargetType(Coord coord,byte targetType)
    {
        //if voxelType does not contain target type, then don't do anything to this voxel.
        if (targetType >behaviorCount)
            return;
        int x = coord.x;
        int y = coord.y;
        byte currentType = currentMap.voxelMap[x, y];

        //generate new voxel transform if null.
        if (voxelTransforms[x, y, targetType] == null)
        {
            GenerateVoxel(coord, targetType);
        }
        else
        {
            //target voxel not null,then turn it to active if it is not.
            if (!voxelTransforms[x, y, targetType].gameObject.activeSelf)
            {
                voxelTransforms[x, y, targetType].gameObject.SetActive(true);
            }
        }
        currentMap.voxelMap[x, y] = targetType;
        //Only set type to targetType and turn current voxel off if currentType != targetType.
        if (currentType != targetType)
        {
            voxelTransforms[x, y, currentType].gameObject.SetActive(false);
        }

    }
    void GenerateVoxel(Coord coord, byte targetType)
    {
        int x = coord.x;
        int y = coord.y;

        Vector3 pos = currentMap.CoordToPosition(coord) * mapUnitSize;
        Transform voxelPrefab = voxelBehaviors[targetType].targetPrefab;

        voxelTransforms[x, y, targetType] = Instantiate(voxelPrefab, pos, Quaternion.identity, voxelBehaviorHolders[targetType]);
        voxelTransforms[x, y, targetType].name = $"Voxel:({x},{y}),Type:{((VoxelBehaviorType)targetType)}";
        voxelTransforms[x, y, targetType].localScale *= mapUnitSize;
    }


    //obstacle
    void GenerateMaze()
    {
        Debug.Log("GenerateMaze");
        byte t = (byte)VoxelBehaviorType.Obstacle;

        int obstacleCount = Mathf.RoundToInt(currentMap.mapSize.x * currentMap.mapSize.y -currentMap.minimunOpenTileCount);
        int currentObstacleCount = 0;

        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = currentMap.GetRandomCoord();
            currentObstacleCount++;
            currentMap.voxelMap[randomCoord.x, randomCoord.y] = t;
            if (randomCoord != currentMap.mapCenterCoord && IsMapFullyAccessible(currentMap.voxelMap, currentObstacleCount))
            {
                float obstacleHeight = Mathf.Lerp(currentMap.minObstacleHeight,currentMap.maxObstacleHeight,
                    CalculateObstacleHeight((float)prng.NextDouble()));
                TurnVoxelToTargetType(randomCoord, t);
                voxelTransforms[randomCoord.x,randomCoord.y,t].localScale =new Vector3(1, obstacleHeight, 1) * mapUnitSize;

                Renderer obstacleRenderer = voxelTransforms[randomCoord.x, randomCoord.y, t].GetComponentInChildren<Renderer>();
                Material obstacleMaterial = new Material(obstacleRenderer.sharedMaterial);
                float colorPercent = (float)randomCoord.y / currentMap.mapSize.y;
                obstacleMaterial.color = Color.Lerp(currentMap.obsForegroundColor, currentMap.obsBackgroundColor, colorPercent);

                obstacleRenderer.sharedMaterial = obstacleMaterial;
            }
            else
            {
                currentObstacleCount--;
                currentMap.voxelMap[randomCoord.x, randomCoord.y] = (byte)VoxelBehaviorType.OpenTile;
            }
        }
    }


    void GenerateMapBoundary()
    {
        boundaryHolder = GenerateHolder("BoundaryHolder");

        float xDistanceToCenter = (float)(maxMapSize.x + currentMap.mapSize.x) / 4 * mapUnitSize;
        float zDistanceToCenter = (float)(maxMapSize.y + currentMap.mapSize.y) / 4 * mapUnitSize;
        float xDif = (float)(maxMapSize.x - currentMap.mapSize.x) / 2 * mapUnitSize;
        float zDif = (float)(maxMapSize.y - currentMap.mapSize.y) / 2 * mapUnitSize;

        Vector3[] maskPositions = {
            new Vector3(xDistanceToCenter,0,0) ,    //0 right 
            new Vector3(-xDistanceToCenter,0,0) ,   //1 left
            new Vector3(0,0,zDistanceToCenter) ,    //2 up
            new Vector3(0,0,-zDistanceToCenter) ,   //3 down
            };

        Transform[] NavmeshMasks = new Transform[4];
        for (int i = 0; i < NavmeshMasks.Length; i++)
        {
            NavmeshMasks[i] = Instantiate(mapBoundaryPrefab, maskPositions[i], Quaternion.identity, boundaryHolder);

            if (i<2)
                NavmeshMasks[i].localScale = new Vector3(xDif, boundaryHeight, maxMapSize.y*mapUnitSize);
            else
                NavmeshMasks[i].localScale = new Vector3(maxMapSize.x*mapUnitSize, boundaryHeight, zDif);
        }


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

    bool IsMapFullyAccessible(byte[,] _voxelMap,int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[_voxelMap.GetLength(0), _voxelMap.GetLength(1)];
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
                        if (neighbourX >= 0 && neighbourX < _voxelMap.GetLength(0) && 
                            neighbourY >= 0 && neighbourY < _voxelMap.GetLength(1))
                        {
                            if (!mapFlags[neighbourX,neighbourY] && _voxelMap[neighbourX,neighbourY] != (byte)VoxelBehaviorType.Obstacle)
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
        if (currentMap.obstacleHeightStep < 1)
        {
            height = currentMap.minObstacleHeight;
        }
        else if (currentMap.obstacleHeightStep < 2)
        {
            height = currentMap.maxObstacleHeight;
        }
        else
        {
            for (float i = 0; i < currentMap.obstacleHeightStep; i++)
            {
                if (height >= i  / currentMap.obstacleHeightStep && height <= (i + 1)  / currentMap.obstacleHeightStep)
                {
                    height = (i + 1)  / currentMap.obstacleHeightStep;
                    break;
                }
            }
        }
        return height;
    }


    public IEnumerator PlayVoxelBehaviorEffect(Coord voxelCoord, VoxelBehavior behavior, params Action<Coord, byte>[] ActionsAfterFlash)
    {
        int x = voxelCoord.x;
        int y = voxelCoord.y;
        float timer = 0f;
        float percentage = 0f;
        byte currentType = currentMap.voxelMap[voxelCoord.x, voxelCoord.y];
        byte targetType = behavior.TargetTypeIndex;

        Transform currentVoxelTransform = voxelTransforms[x, y, currentType];


        if (targetType <=currentType)//play flash effect
        {
            //get Transform, Material and Color of CURRENT voxel
            Renderer voxelRenderer = currentVoxelTransform.GetComponentInChildren<Renderer>();
            Material tileMat = voxelRenderer.material;
            Color initialColor = tileMat.color;
            //start flashing and set this voxel to occupied state ,so it won't be selected when it is flashing.
            currentMap.voxelMap[x, y] = voxelOccupied;

            while (timer < behavior.effectLastTime)
            {
                timer += Time.deltaTime;
                percentage = timer / behavior.effectLastTime;
                tileMat.color = Color.Lerp(initialColor, behavior.effectColor
                    , Mathf.PingPong(percentage * behavior.flashTimes * 2, 1f));
                yield return null;
            }
            tileMat.color = initialColor;
        }
        else if (targetType > currentType)//play translucent effect
        {
            //get Transform, Material and Color of TARGET voxel
            TurnVoxelToTargetType(voxelCoord, targetType);
            Transform targetVoxelTransform = voxelTransforms[x, y, targetType];
            //start flashing and set this voxel to occupied state ,so it won't be selected when it is flashing.
            currentMap.voxelMap[x, y] = voxelOccupied;
            //maintain current voxel transform active for translucent effect duration;
            currentVoxelTransform.gameObject.SetActive(true);

            //make the target transform translucent and disable its collider
            Renderer voxelRenderer = targetVoxelTransform.GetComponentInChildren<Renderer>();
            Collider voxelCollider = targetVoxelTransform.GetComponent<Collider>();
            voxelCollider.enabled = false;

            Material tileMat = voxelRenderer.material;
            Color initialColor = tileMat.color;
            tileMat.color = Color.Lerp(initialColor, behavior.effectColor, 0.5f);
            yield return new WaitForSeconds(behavior.effectLastTime);

            voxelCollider.enabled = true;
            tileMat.color = initialColor;
            currentVoxelTransform.gameObject.SetActive(false);
            //reset voxel state to target type
        }
        currentMap.voxelMap[x, y] = currentType;

        //actions after effect
        if (ActionsAfterFlash != null)
        {
            for (int i = 0; i < ActionsAfterFlash.Length; i++)
            {
                ActionsAfterFlash[i].Invoke(voxelCoord, behavior.TargetTypeIndex);
            }
        }
        yield return null;
    }

}

