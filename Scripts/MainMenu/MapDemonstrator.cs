using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDemonstrator : MonoBehaviour
{
    public float demoMapRoutateSpeed;
    public Camera demoCam;

    public int demoEnemyCount;
    EnemyController enemyPrefab;
    MapManager mapManager;


    // Start is called before the first frame update
    void Start()
    {
        demoCam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        demoCam.transform.RotateAround(Vector3.zero, transform.up, demoMapRoutateSpeed * Time.deltaTime);
    }
}
