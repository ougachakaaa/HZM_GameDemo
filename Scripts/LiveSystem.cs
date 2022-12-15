using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveSystem : MonoBehaviour
{
    public EnemySpawner _enemySpawner;

    //Hp system
    public float maxHp;
    public float _currentHp;
    public float CurrentHp
    {
        get { return _currentHp; }
        set { _currentHp = Mathf.Clamp(value, 0, maxHp); }
    }

    //damage
    public float defensePoint;


    private void Start()
    {
        CurrentHp = maxHp;
        _enemySpawner = FindObjectOfType<EnemySpawner>();
    }
    public void Heal(float percentage)
    {
        CurrentHp += percentage / 100 * maxHp;
    }

    public void TakeDamage(float damagePoint)
    {
        CurrentHp -= damagePoint-defensePoint;
        if (CurrentHp <= 0)
            Dead();
    }

    void Dead()
    {
        //Actions.OnEnemyKilled?.Invoke();
        _enemySpawner.enmeyList.Remove(this.transform);
        _enemySpawner.UpdateEnemyList();
        Destroy(gameObject);
    }


}
