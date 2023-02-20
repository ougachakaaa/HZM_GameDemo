using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLivingEntity : LivingEntity
{
    public HpBar hpBar;
    public ExpBar expBar;
    
    public int currentLevel = 1;
    public int currentExpPoint =0;
    public int expToLevelUp = 3;
    public int levelExpStep = 2;

    public int ATKincreasment = 5;
    public int DEFincreasment = 3;

    public float levelUpHealPercentage;

    private void OnEnable()
    {
        Actions.OnEnemyAttack += TakeDamage;
        Actions.OnCollected += DiamondCollected;
    }
    private void OnDisable()
    {
        Actions.OnEnemyAttack -= TakeDamage;
        Actions.OnCollected -= DiamondCollected;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Actions.IsPlayerDead = isDead;
        hpBar.UpdateHpBar();
    }

    public override void TakeDamage(float damage)
    {

        base.TakeDamage(damage);
        hpBar.UpdateHpBar();

    }
    public override void Heal(float percentage)
    {
        base.Heal(percentage);
        hpBar.UpdateHpBar();

    }
    public override void Die()
    {
        base.Die();
        Actions.IsPlayerDead = isDead;
        Actions.OnPlayerDie.Invoke();
    }

    public void LevelUpCheck(int expPoint)
    {
        currentExpPoint += expPoint;
        if (currentExpPoint >= expToLevelUp)
        {
            //set next level
            currentLevel++;
            currentExpPoint %= expToLevelUp;
            expToLevelUp += levelExpStep;

            maxHp += 10;
            Heal(levelUpHealPercentage);
            Actions.OnLevelUp?.Invoke();
        }
        expBar.UpdateExpBar();
    }

    public void DiamondCollected(Collection c)
    {
        if (c is DiamondCollection)
        {
            LevelUpCheck((c as DiamondCollection).diamondValue);
        }
    }

    public void UpgradeATK()
    {
        Debug.Log("ATK UP");
        attackPoint += ATKincreasment;
    }
    public void UpgradeDEF()
    {
        Debug.Log("DEF UP");
        defensePoint += DEFincreasment;
    }

}
