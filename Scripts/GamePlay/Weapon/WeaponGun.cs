using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGun : Weapon
{
    public WeaponGun_Param[] levelParams;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        maxLevel = levelParams.Length;
        currentLevel = 0;

        UpgradeWeapon();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Actions.IsPlayerDead)
        {
            ShootCheck();
        }
    }
    public void ShootCheck()
    {
        if (_playerCharacterController.canShoot && fireTimer > fireDuration)
        {
            Vector3 muzzlePos = muzzle.position;
            Quaternion dir = Quaternion.LookRotation(muzzle.forward);
            FireProjectile(projectilePool.Get(), muzzlePos, dir);

            fireTimer = 0f;
        }
        else
        {
            fireTimer += Time.deltaTime;
        }
    }

    public void UpgradeWeapon()
    {
        Debug.Log("WeaponGun LevelUP");
        if (currentLevel<maxLevel)
        {
            damageFactor = levelParams[currentLevel].damageFactor;
            maxCollideCount = levelParams[currentLevel].maxColliderCount;
            fireDuration = levelParams[currentLevel].fireDuration;

            currentLevel++;
        }
    }

    [System.Serializable]
    public struct WeaponGun_Param
    {
        public float damageFactor;
        public int maxColliderCount;
        public float fireDuration;
    }
}

