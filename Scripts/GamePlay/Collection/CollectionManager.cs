using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance;
    public MapManager mapManager;

    public List<Collection> collectionPrefabs;
    public ObjectPool<Collection>[] collectionPools;
    Func<Collection>[] PoolCreateFuncs; 
    Action<Collection>[] PoolReleaseActs; 
    [SerializeField] int defaultCapacity = 30;
    [SerializeField] int maxSize = 50;


    public Transform[] collectionHolders;

    //diamond
    public bool isGeneratingDiamond = false;
    public float diamondDuration;
    public int maxDiamondCount;
    float diamondTimer;
    int currentDiamondCount =0;

    // Start is called before the first frame update

    private void Start()
    {
        switch (mapManager.gameMode)
        {
            case GameMode.Survive:
                break;
            case GameMode.DiamondCollect:
                isGeneratingDiamond = true;
                break;
            case GameMode.Demo:
                break;
            default:
                break;
        }

    }
    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        int count = collectionPrefabs.Count;


        collectionHolders = new Transform[count];
        collectionPools = new ObjectPool<Collection>[count];
        PoolCreateFuncs = new Func<Collection>[count];
        PoolReleaseActs = new Action<Collection>[count];

        PoolCreateFuncs[(int)collectionPrefabs[0].collectionType] = OnCreateCoinItem;//0:coin
        PoolCreateFuncs[(int)collectionPrefabs[1].collectionType] = OnCreateDiamondItem;//1:diamond

        foreach (Collection c in collectionPrefabs)
        {
            int i = (int)c.collectionType;
            collectionHolders[i] = new GameObject("Pool:" + c.name).transform;
            collectionHolders[i].parent = transform;
            collectionPools[i] = new ObjectPool<Collection>(PoolCreateFuncs[i], OnGetPoolItem, OnReleaseItem, OnDestroyPoolItem, true, defaultCapacity, maxSize);
        }

        diamondTimer = Time.time;
        Actions.OnEnemyDie += GenerateCoin;
    }

    private void OnDisable()
    {
        Actions.OnEnemyDie -= GenerateCoin;
    }

    private void Update()
    {
        GenerateDiamondCheck();
    }

    void GenerateCoin(EnemyController enemy)
    {
        int type = (int)CollectionType.Coin;
        Vector3 coinPos = enemy.transform.position + Vector3.up;
        Collection coin = collectionPools[type].Get();
        Debug.Log("objpool of " + type + " get");

        coin.transform.position = coinPos;
    }
    void GenerateDiamondCheck()
    {
        if (isGeneratingDiamond && Time.time > diamondTimer+diamondDuration && currentDiamondCount < maxDiamondCount)
        {
            int type = (int)CollectionType.Diamond;
            Coord coord = mapManager.currentMap.GetRandomCoordOfType((byte)VoxelBehaviorType.OpenTile);
            Vector3 pos = mapManager.currentMap.CoordToPosition(coord)*mapManager.mapUnitSize + Vector3.up;
            Collection diamond = collectionPools[type].Get();
            diamond.transform.position = pos;
            currentDiamondCount++;

            diamondTimer = Time.time;
        }

    }

    #region Pool stuff
    private void OnDestroyPoolItem(Collection obj)
    {
        Destroy(obj);
    }

    private void OnReleaseCoinItem(Collection obj)
    {
        obj.gameObject.SetActive(false);
    }
    private void OnReleaseItem(Collection obj)
    {
        if (obj is DiamondCollection)
        {
            currentDiamondCount--;
        }
        obj.gameObject.SetActive(false);
    }

    private void OnGetPoolItem(Collection obj)
    {

        if (!obj.gameObject.activeSelf)
        {
            obj.gameObject.SetActive(true);
        }
    }

    private Collection OnCreateCoinItem()
    {
        int t = (int)CollectionType.Coin;
        Collection c = Instantiate(collectionPrefabs[t],collectionHolders[t]);
        return c;
    }
    private Collection OnCreateDiamondItem()
    {
        int t = (int)CollectionType.Diamond;
        Collection c = Instantiate(collectionPrefabs[t], collectionHolders[t]);
        return c;
    }
    #endregion 

}
