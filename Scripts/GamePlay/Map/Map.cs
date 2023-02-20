using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Map", menuName = "Map Element/Map")]
public class Map : ScriptableObject
{
    public string mapName;
    public int mapIndex;
    public Vector2Int mapSize;

    //voxelDatas
    public int voxelCount { get => voxelMap.Length; }
    public byte[,] voxelMap;
    public List<Coord> allVoxelCoords { private set; get; }
    public Queue<Coord> shuffledVoxelCoords { private set; get; }


    //map size
    public Coord mapCenterCoord{get => new Coord(mapSize.x / 2, mapSize.y / 2);}
    public Vector3 mapCenterPos{get => new Vector3(mapSize.x / 2f, 0f, mapSize.y / 2f);}

    //obstacles
    public float maxObstacleHeight =4f;
    public float minObstacleHeight =8f;
    public int obstacleHeightStep =4;

    public Color obsForegroundColor;
    public Color obsBackgroundColor;
    public int obstacleSeed;
    public int shuffleSeed;
    public int minimunOpenTileCount;

  //a scriptable object file is an instance of the class, any modification to this instance will change the data this file contains. be careful with these field if you don't want to change the original data of this file
    public void InitializeMap()
    {
        allVoxelCoords?.Clear();
        shuffledVoxelCoords?.Clear();
        voxelMap = new byte[mapSize.x, mapSize.y];
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                voxelMap[x, y] = (byte)VoxelBehaviorType.OpenTile;
            }
        }
        allVoxelCoords = new List<Coord>();
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                allVoxelCoords.Add(new Coord(x, y));
            }
        }
        ShuffleVoxelCoords(shuffleSeed);
    }


    public void PrintMapInfo()
    {
        Debug.Log($"Name:{mapName}");
        Debug.Log($"Index:{mapIndex}");
        Debug.Log($"Size:{mapSize}");
        Debug.Log($"VoxelMap:{voxelMap} of length:{voxelMap.Length}");
        Debug.Log($"Size:{voxelCount}");

    }
    public int CheckVoxelCountOfType(byte type)
    {
        int count =0;
        foreach (byte t in voxelMap)
        {
            if (t == type)
            {
                count++;
            }
        }
        return count;
    }
    //get random available coord of certain type
    public Coord GetRandomCoordOfType(byte type)
    {
        Coord coord;
        for (int i = 0; i < voxelCount; i++)
        {
            coord = GetRandomCoord();
            if (voxelMap[coord.x, coord.y] == type)
                return coord;
        }
        coord = GetRandomCoord();
        Debug.Log($"no Type:{type} found, return random coord");
        return coord;
    }

    //get random available coord, exclude the occupied voxel.
    public Coord GetRandomCoord()
    {
        Coord coord;
        foreach (Coord c in shuffledVoxelCoords)
        {
            if (voxelMap[c.x,c.y] != MapManager.voxelOccupied)
            {
                coord = shuffledVoxelCoords.Dequeue();
                shuffledVoxelCoords.Enqueue(coord);
                return coord;
            }
        }
        Debug.Log($"no available found, return map center coord");
        return mapCenterCoord;

    }

    //position coord transition
    public Vector3 CoordToPosition(Coord coord)
    {
        return new Vector3(0.5f + coord.x, 0, 0.5f + coord.y) - mapCenterPos;
    }
    public Coord PositionToCoord(Vector3 position)
    {
        Vector3 pos = position + mapCenterPos;
        int x = Mathf.RoundToInt(pos.x - 0.5f);
        int y = Mathf.RoundToInt(pos.y - 0.5f);
        return new Coord(x, y);
    }
    void ShuffleVoxelCoords(int seed)
    {
        shuffledVoxelCoords = new Queue<Coord>(Utility.ShuffleArray<Coord>(allVoxelCoords.ToArray(),seed));
    }
}
public struct Coord
{
    public int x;
    public int y;
    public Coord(int _x, int _y)
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
        return !(c1.x == c2.x && c1.y == c2.y);
    }

}

