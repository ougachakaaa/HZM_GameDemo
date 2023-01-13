using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLivingEntity : LivingEntity
{
    public HpBar hpbar;
    private void OnEnable()
    {
        Actions.OnEnemyAttack += TakeDamage;
    }
    private void OnDisable()
    {
        Actions.OnEnemyAttack -= TakeDamage;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Actions.IsPlayerDead = isDead;
        hpbar.UpdateHpBar();
    }

    public override void TakeDamage(float damage)
    {

        base.TakeDamage(damage);
        hpbar.UpdateHpBar();

    }
    public override void Die()
    {
        base.Die();
        Actions.IsPlayerDead = isDead;
        Actions.OnPlayerDie.Invoke();
    }


}
