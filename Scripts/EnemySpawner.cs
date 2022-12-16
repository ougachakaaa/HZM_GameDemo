using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefabs;
    public Transform spawnCenter;
    public float innerSpawnRange = 10;
    public float outterSpawnRange = 20;
    public StarterAssetsInputs _input;

    public List<Transform> enmeyList = new List<Transform>();
    public int currentEnemyCount;

    //using actions
    private void OnEnable()
    {
        Actions.OnEnemyKilled += UpdateEnemyList;
    }
    private void OnDisable()
    {
        
        Actions.OnEnemyKilled -= UpdateEnemyList;
    }



    void Update()
    {
        if (_input.spawn)
        {
            Vector3 spawnPos = GetSpawnPos(spawnCenter.position);
            GameObject thisEnmey = Instantiate(enemyPrefabs, spawnPos, Quaternion.identity, transform);
            enmeyList.Add(thisEnmey.transform);
            _input.spawn = false;
            UpdateEnemyList();
        }


    }

    public Vector3 GetSpawnPos(Vector3 _SpawnCenter)
    {
        Vector3 spawnPos = new Vector3(0,0,0);
        RaycastHit hit;
        Vector2 _randomPointInCircle = GetRandomPointInCircle(innerSpawnRange, outterSpawnRange);
        float xPos = _SpawnCenter.x + _randomPointInCircle.x;
        float zPos = _SpawnCenter.z + _randomPointInCircle.y;
        Ray _ray = new Ray(new Vector3(xPos,100,zPos),Vector3.down);
        if (Physics.Raycast(_ray, out hit, 200f))
            spawnPos = hit.point;

        return spawnPos;
    }
    public Vector2 GetRandomPointInCircle(float _innerRange,float _outterRange)
    {
        float radius = Random.Range(_innerRange, _outterRange);
        float degree = Random.Range(-180, 180f);
        float x = radius * Mathf.Sin(degree);
        float z = radius * Mathf.Cos(degree);
        return new Vector2(x,z);
    }
    public void UpdateEnemyList()
    {
        if(enmeyList != null)
        {
            foreach (Transform _enmeyTransform in enmeyList)
            {
                if (_enmeyTransform == null)
                {
                    enmeyList.Remove(_enmeyTransform);
                }
            }
            currentEnemyCount = enmeyList.Count;
        }
        else
        {
            currentEnemyCount = 0;
        }
        Debug.Log($"We now have {currentEnemyCount} enemy in the scene");
    }

}
