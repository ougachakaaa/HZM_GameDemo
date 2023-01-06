using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour,IDamagable
{
    protected bool isDead;
    [SerializeField]protected float maxHp;
    float currentHp;
    public float CurrentHp
    {
        get { return currentHp; }
        set { currentHp = Mathf.Clamp(value, 0, maxHp); }
    }

    [SerializeField] protected float defensePoint;

    protected virtual void Start()
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

    protected virtual void Die()
    {
        isDead = true;
    }

}
