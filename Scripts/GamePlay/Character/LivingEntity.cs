using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour,IDamagable
{
    //Hp
    public bool isDead;
    [SerializeField]public float maxHp;
    float currentHp;
    public float CurrentHp
    {
        get { return currentHp; }
        set { currentHp = Mathf.Clamp(value, 0, maxHp); }
    }
    [SerializeField] protected float defensePoint;

    //ATK
    public float attackPoint;


    protected virtual void Start()
    {
        InitializeLivingEntity();
    }
    public void InitializeLivingEntity()
    {
        isDead = false;
        CurrentHp = maxHp;
    }

    public void Heal(float percentage)
    {
        CurrentHp += percentage / 100 * maxHp;
    }

    public virtual void TakeDamage(float damage)
    {
        CurrentHp -= damage-defensePoint;
        if (CurrentHp <= 0 && !isDead)
            Die();
    }

    public virtual void Die()
    {
        isDead = true;
    }

}
