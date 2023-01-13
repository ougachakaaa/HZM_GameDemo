using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //alway update when parameters change;
    [HideInInspector][SerializeField]
    private bool _alwayUpdate;


    public Transform tilePrefab;
    public Vector2 mapSize;
    [Range(0,1)] public float outlineSize;

    public Transform[,] tiles;

    [TextArea()]
    string str;

    public void GenerateMap()
    {
        Debug.Log("GenerateMap");
        string holderName = "GeneratedMap";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }
        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        tiles = new Transform[(int)mapSize.x, (int)mapSize.y];
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePos = new Vector3(-mapSize.x / 2 + x, 0, -mapSize.y / 2 + y);
                tiles[x, y] = Instantiate<Transform>(tilePrefab, tilePos, Quaternion.Euler(Vector3.right*90), mapHolder);
                tiles[x, y].name = $"Tile:({x},{y})";
                tiles[x, y].localScale = (1 - outlineSize) * Vector3.one;
            }
        }
    }
}
