using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using System.Threading;

public class EnemySpawner : MonoBehaviour
{
    public EnemyController[] enemyPrefabs;
    public Transform PlayerTransform;
    public StarterAssetsInputs _input;

    //enemy spawn
    [Header("Spawn Parameters")]
    public float innerSpawnRange = 15;
    public float outterSpawnRange = 30;
    [SerializeField] float spawnRate =1;
    bool isSpawningEnemy;
    float enemySpawnTimer;

    public List<Transform> enmeyList = new List<Transform>();
    public int currentEnemyCount;

    //using actions
    private void OnEnable()
    {
        Actions.OnEnemyDie += UpdateEnemyList;
    }
    private void OnDisable()
    {
        Actions.OnEnemyDie -= UpdateEnemyList;
    }

    private void Start()
    {
        isSpawningEnemy = false;
    }

    void Update()
    {
        if (_input.spawn)
        {
            isSpawningEnemy = !isSpawningEnemy;
            enemySpawnTimer = 0;
            _input.spawn = false;
        }

        if (isSpawningEnemy)
        {
            if (enemySpawnTimer>1/spawnRate)
            {
                SpawnEnemy(0);
                enemySpawnTimer = 0;
            }
            enemySpawnTimer += Time.deltaTime;
        }

    }
    void SpawnEnemy(int enemyID)
    {

        Vector3 spawnPos = GetSpawnPos(PlayerTransform.position);
        EnemyController t = Instantiate(enemyPrefabs[enemyID], spawnPos, Quaternion.identity, transform);
        enmeyList.Add(t.transform);
    }
    Vector3 GetSpawnPos(Vector3 _SpawnCenter)
    {
        Vector3 spawnPos = new Vector3(0,0,0);
        RaycastHit hit;

        Vector2 a = Random.insideUnitCircle;
        Vector2 _randomPointInCircle = GetRandomPointInCircle(innerSpawnRange, outterSpawnRange);
        float xPos = _SpawnCenter.x + _randomPointInCircle.x;
        float zPos = _SpawnCenter.z + _randomPointInCircle.y;
        Ray _ray = new Ray(new Vector3(xPos,100,zPos),Vector3.down);
        if (Physics.Raycast(_ray, out hit, 200f))
            spawnPos = hit.point;

        return spawnPos;
    }
    Vector2 GetRandomPointInCircle(float _innerRange,float _outterRange)
    {
        float radius = Random.Range(_innerRange, _outterRange);
        float degree = Random.Range(-180, 180f);
        float x = radius * Mathf.Sin(degree);
        float z = radius * Mathf.Cos(degree);
        return new Vector2(x,z);
    }

    public Vector3 FindClosestEnemy()
    {
        Vector3 pos = new Vector3(0, 100, 0);
        float closestDistance = 10000f;
        if(enmeyList != null && enmeyList.Count > 0)
        {
            foreach (Transform enemy in enmeyList)
            {
                float thisDistance =(enemy.position - PlayerTransform.position).sqrMagnitude;
                if (thisDistance < closestDistance)
                {
                    closestDistance = thisDistance;
                    pos = enemy.position;
                }
            }
        }
        return pos;
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
        Debug.Log($"We now have {currentEnemyCount} enemy in the scene");
    }

}
