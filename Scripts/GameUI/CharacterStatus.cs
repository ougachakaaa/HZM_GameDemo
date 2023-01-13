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


    private void OnEnable()
    {
        Actions.OnEnemyDie += EnemyCountIncrese;
        Actions.OnLootCollected += CoinCollectedIncrese;
    }
    private void OnDisable()
    {
        Actions.OnEnemyDie -= EnemyCountIncrese;
        Actions.OnLootCollected += CoinCollectedIncrese;
    }
    // Start is called before the first frame update
    void Start()
    {
        enemyKilledCount = 0;
        coinCollectedCount = 0;
    }

    // Update is called once per frame

    void EnemyCountIncrese(EnemyController _enemyController)
    {
        enemyKilledCount++;
        enemyKilledText.text = enemyKilledCount.ToString();
    }

    public void CoinCollectedIncrese(Loot loot)
    {
        coinCollectedCount++;
        coinCollectedText.text = coinCollectedCount.ToString();
    }

}
