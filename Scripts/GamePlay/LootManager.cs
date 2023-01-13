using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class LootManager : MonoBehaviour
{
    public static LootManager Instance;

    public List<Loot> lootPrefabs;
    Dictionary<Loot, ObjectPool<Loot>> lootPoolDict = new Dictionary<Loot, ObjectPool<Loot>>();
    Dictionary<Loot, GameObject> lootHolders = new Dictionary<Loot, GameObject>();
    [SerializeField] int defaultCapacity = 30;
    [SerializeField] int maxSize = 50;

    Vector3 GeneratePos;

    private void Start()
    {
        Debug.Log("Start here");
    }
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        lootPoolDict?.Clear();
        lootHolders?.Clear();

        foreach (Loot prefab in lootPrefabs)
        {
            GameObject holder = new GameObject("Pool:" + prefab.name);
            holder = Instantiate(holder, transform);
            lootHolders.Add(prefab, holder);
            ObjectPool<Loot> pool = new ObjectPool<Loot>(OnCreatePoolItem,OnGetPoolItem,OnReleasePoolItem,OnDestroyPoolItem,false,defaultCapacity,maxSize);
            lootPoolDict.Add(prefab,pool);
        }

        Actions.OnLootCollected += lootPoolDict[lootPrefabs[0]].Release;
        Actions.OnEnemyDie += GenerateLoot;
    }
    private void OnDestroy()
    {
        Actions.OnLootCollected -= lootPoolDict[lootPrefabs[0]].Release;
        Actions.OnEnemyDie -= GenerateLoot;
    }

    #region pool action
    private void OnDestroyPoolItem(Loot obj)
    {
        Destroy(obj.gameObject);
    }

    private void OnReleasePoolItem(Loot obj)
    {
        obj.isCollected = false;
        obj.gameObject.SetActive(false);
    }

    private void OnGetPoolItem(Loot obj)
    {
        obj.gameObject.SetActive(true);
        obj.transform.position = GeneratePos;
        obj.SwitchAnimation(true);
    }

    private Loot OnCreatePoolItem()
    {
        int lootID = 0;
        Loot lootItem = lootPrefabs[lootID];
        Transform holder = lootHolders[lootPrefabs[lootID]].transform;

        Loot thisLoot = Instantiate(lootItem, holder);
        thisLoot.transform.position = GeneratePos;
        return thisLoot;

    }
    #endregion

    void GenerateLoot(EnemyController enemy)
    {
        GeneratePos = enemy.transform.position + Vector3.up;
        Loot thisLoot = lootPoolDict[lootPrefabs[0]].Get();
    }

}
