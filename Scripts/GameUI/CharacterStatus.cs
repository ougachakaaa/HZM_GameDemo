using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterStatus : MonoBehaviour
{

    public int enemyKilledCount;
    [SerializeField] TextMeshProUGUI enemyKilledText;

    public int coinCollectedCount;
    [SerializeField] TextMeshProUGUI coinCollectedText;
    
    public float enemySpawnRate;
    [SerializeField] TextMeshProUGUI enemySpawnRateText;


    private void OnEnable()
    {
        Actions.OnEnemyDie += EnemyCountIncrease;
        Actions.OnCollected += CoinCollected;
        Actions.OnEnemySpawnRateIncrease += EnemySpawnRateIncrease;
    }
    private void OnDisable()
    {
        Actions.OnEnemyDie -= EnemyCountIncrease;
        Actions.OnCollected -= CoinCollected;
        Actions.OnEnemySpawnRateIncrease -= EnemySpawnRateIncrease;
    }
    // Start is called before the first frame update
    void Start()
    {
        enemyKilledCount = 0;
        coinCollectedCount = 0;
    }

    // Update is called once per frame

    void EnemyCountIncrease(EnemyController _enemyController)
    {
        enemyKilledCount++;
        enemyKilledText.text = enemyKilledCount.ToString();
    }

    public void CoinCollected(Collection c)
    {
        if (c is CoinCollection)
        {
            coinCollectedCount += (c as CoinCollection).coinValue;
            coinCollectedText.text = coinCollectedCount.ToString();
        }
    }
    public void EnemySpawnRateIncrease(float rate)
    {
        enemySpawnRate = rate;
        enemySpawnRateText.text = $"Enemy: {enemySpawnRate:F1}/S";
    }

}
