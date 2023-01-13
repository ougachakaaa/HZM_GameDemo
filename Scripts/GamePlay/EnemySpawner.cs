using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using StarterAssets;

public class EnemySpawner : MonoBehaviour
{
    public Transform PlayerTransform;
    public StarterAssetsInputs _input;
    PlayerLivingEntity _playerLivingEntity;

    [SerializeField] List<GameObject> enemyPrefabs;


    //enemy spawn
    [Header("Spawn Parameters")]
    public float innerSpawnRange = 15;
    public float outterSpawnRange = 30;
    [SerializeField] float spawnRate =1;
    [SerializeField] LayerMask spawnCheckLayer;
    bool isSpawningEnemy;
    float enemySpawnTimer;

    public List<Transform> activeEnemyList = new List<Transform>();
    public int currentEnemyCount;

    //using actions
    [Header("Object Pool")]
    [SerializeField] EnemyController enemyPrefab;
    [SerializeField] int defaultCapacity = 20;
    [SerializeField] int maxSize = 100;

    ObjectPool<EnemyController> enemyPool;
    public static EnemySpawner Instance;
    private void Awake()
    {

        //clear list or pool
        enemyPool?.Clear();
        activeEnemyList?.Clear();

        //singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        enemyPool = new ObjectPool<EnemyController>(OnCreatePoolItem, OnGetPoolItem, OnReleasePoolItem, OnDestroyPoolItem, false, defaultCapacity, maxSize);



        _playerLivingEntity = PlayerTransform.GetComponent<PlayerLivingEntity>();

        isSpawningEnemy = false;

        //action
        Actions.OnEnemyDie += enemyPool.Release;
    }
    private void OnDisable()
    {
        //unsubscribe action
        Actions.OnEnemyDie -= enemyPool.Release;
    }

    void Update()
    {
        if (_input.spawn)
        {
            isSpawningEnemy = !isSpawningEnemy;
            enemySpawnTimer = 0;
            _input.spawn = false;
        }

        if (isSpawningEnemy && _playerLivingEntity.isDead == false)
        {
            if (enemySpawnTimer>1/spawnRate)
            {

                enemyPool.Get();
                enemySpawnTimer = 0;
            }
            enemySpawnTimer += Time.deltaTime;
        }

    }
    //spawn
    EnemyController SpawnEnemy()
    {
        Vector3 spawnPos = GetSpawnPos(PlayerTransform.position);
        EnemyController thisEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, transform);
        return thisEnemy;
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
        if (Physics.Raycast(_ray, out hit, 200f,spawnCheckLayer))
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
        if(activeEnemyList != null && activeEnemyList.Count > 0)
        {
            for (int i = 0; i < activeEnemyList.Count; i++)
            {

                float thisDistance = (activeEnemyList[i].position - PlayerTransform.position).sqrMagnitude;
                if (thisDistance < closestDistance)
                {
                    closestDistance = thisDistance;
                    pos = activeEnemyList[i].position;
                }
            }
        }
        return pos;
    }



    //enmeyPool
    private void OnDestroyPoolItem(EnemyController obj)
    {
        Destroy(obj);
    }

    private void OnReleasePoolItem(EnemyController obj)
    {
        obj.ResetEnemyController();
        activeEnemyList.Remove(obj.transform);
        obj.gameObject.SetActive(false);

    }

    private void OnGetPoolItem(EnemyController obj)
    {
        obj.transform.position = GetSpawnPos(PlayerTransform.position);

        obj.gameObject.SetActive(true);
        obj.InitEnemyController();

        activeEnemyList.Add(obj.transform);
    }

    private EnemyController OnCreatePoolItem()
    {
        return SpawnEnemy();
    }
}
