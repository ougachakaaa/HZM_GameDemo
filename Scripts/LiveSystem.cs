using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveSystem : MonoBehaviour
{
    public EnemySpawner _enemySpawner;

    //actions

    //Hp system
    public EnemyController _enemyController;

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
        _enemyController = GetComponent<EnemyController>();
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
        _enemyController.BeingHit();
    }

    void Dead()
    {
        //Actions.OnEnemyKilled?.Invoke();
        _enemySpawner.enmeyList.Remove(this.transform);
        _enemySpawner.UpdateEnemyList();
        Destroy(gameObject);
    }


}
