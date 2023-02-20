using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using StarterAssets;

enum SpawnType
{
    CreateNew,
    GetFromPool,
}
public class EnemySpawner : MonoBehaviour
{
    public Transform PlayerTransform;
    public StarterAssetsInputs _input;
    public MapManager mapManager;
    PlayerLivingEntity playerLivingEntity;

    [SerializeField] List<GameObject> enemyPrefabs;
    public Transform enemyHolder;


    //enemy spawn
    [Header("Spawn Parameters")]
    public float innerSpawnRange = 15;
    public float outterSpawnRange = 30;
    [SerializeField] float initialSpawnRate =1f;
    [SerializeField] float spawnRateStep =1f;
    [SerializeField] float spawnRateIncreaseDurantion =15;
    public float spawnRate;
    float spawnRateTimer =0f;
    [SerializeField] Color spawnEffectColor;
    [SerializeField] LayerMask spawnCheckLayer;
    bool isSpawningEnemy;
    float enemySpawnTimer;

    public List<Transform> activeEnemyList = new List<Transform>();
    int currentEnemyCount;
    public int MaxEnemyCount = 40;

    //using actions
    [Header("Object Pool")]
    [SerializeField] EnemyController enemyPrefab;
    [SerializeField] int defaultCapacity = 40;
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



        playerLivingEntity = PlayerTransform.GetComponent<PlayerLivingEntity>();
        enemyHolder = new GameObject("EnemyHolder").transform;

        isSpawningEnemy = false;
        spawnRate = initialSpawnRate;

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
            Actions.OnEnemySpawnRateIncrease?.Invoke(spawnRate);

        }

        CalculateSpawnRate();
        EnemySpawnCheck();


    }

    void EnemySpawnCheck()
    {
        if (isSpawningEnemy && playerLivingEntity.isDead == false && currentEnemyCount < MaxEnemyCount)
        {
            if (enemySpawnTimer > 1 / spawnRate)
            {
                enemySpawnTimer = 0;
                Coord spawnCoord = mapManager.currentMap.GetRandomCoordOfType((byte)VoxelBehaviorType.OpenTile);
                StartCoroutine(mapManager.PlayVoxelBehaviorEffect(spawnCoord, mapManager.voxelBehaviors[(byte)VoxelBehaviorType.EnemySpawn], GenerateEnemyByCoord));
            }
            enemySpawnTimer += Time.deltaTime;
        }
    }
    //spawn
    void GenerateEnemyByCoord(Coord coord,byte _placeHolder)
    {
        EnemyController thisEnemy = enemyPool.Get();
        thisEnemy.transform.position = mapManager.currentMap.CoordToPosition(coord)*mapManager.mapUnitSize;
        if (enemyHolder != null)
        {
            if (thisEnemy.transform.parent != enemyHolder)
            {
                thisEnemy.transform.parent = enemyHolder;
            }
        }
        else
        {

        }
    }

    public void CalculateSpawnRate()
    {
        if (isSpawningEnemy)
        {
            spawnRateTimer += Time.deltaTime;
            if (spawnRateTimer > spawnRateIncreaseDurantion)
            {
                spawnRate += spawnRateStep;
                Actions.OnEnemySpawnRateIncrease?.Invoke(spawnRate);
                spawnRateTimer = 0f;
            }
        }
    }

    
    Vector3 GetSpawnPos(Coord spawnCoord)
    {
        return mapManager.currentMap.CoordToPosition(spawnCoord);
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



    #region enmeyPool stuff
    private void OnDestroyPoolItem(EnemyController obj)
    {
        Destroy(obj);
    }

    private void OnReleasePoolItem(EnemyController obj)
    {
        activeEnemyList.Remove(obj.transform);
        obj.gameObject.SetActive(false);

    }

    private void OnGetPoolItem(EnemyController obj)
    {
        obj.gameObject.SetActive(true);
        obj.InitEnemyController();
        activeEnemyList.Add(obj.transform);
    }

    private EnemyController OnCreatePoolItem()
    {
        return Instantiate(enemyPrefab);
    }
    #endregion 

}
