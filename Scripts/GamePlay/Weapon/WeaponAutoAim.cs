using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAutoAim : Weapon
{
    public bool isAutoShooting;

    int projectilePerRound;
    protected int projectileThisRound;

    protected float roundTimer;
    [SerializeField]float roundDuration = 1f;

    public float muzzleHeight;

    protected EnemySpawner _spawner;

    public Transform rotateCenter;

    //upgrade
    public WeaponAutoAim_Param[] levelParams;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        projectileThisRound = 0;
        _spawner = FindObjectOfType<EnemySpawner>();

        maxLevel = levelParams.Length;
        currentLevel = 0;
        muzzle.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentLevel>0)
        {
            if (!_playerLivingEntity.isDead && isAutoShooting && _spawner.activeEnemyList.Count > 0)
            {
                if (projectileThisRound < projectilePerRound)
                {
                    if (fireTimer > fireDuration)
                    {
                        Vector3 targetPos = _spawner.FindClosestEnemy();
                        Quaternion dir = Quaternion.LookRotation(targetPos - _playerCharacterController.transform.position);
                        Vector3 muzzlePos = GetMuzzleOffset(targetPos) + transform.position;
                        muzzle.position = muzzlePos;
                        FireProjectile(projectilePool.Get(), muzzlePos, dir);

                        fireTimer = 0f;
                        projectileThisRound++;
                    }
                }
                if (roundTimer>roundDuration)
                {
                    projectileThisRound = 0;
                    roundTimer = 0f;
                }
                fireTimer += Time.deltaTime;
                roundTimer += Time.deltaTime;
            }
            else
            {
                muzzle.position = transform.position + Vector3.up * 2.5f;
            }
        }

           
    }
    public Vector3 GetMuzzleOffset(Vector3 target)
    {
        Vector3 muzzleOffset = Vector3.ProjectOnPlane(target - transform.position, transform.up).normalized / 2;
        muzzleOffset.y = muzzleHeight;
        return muzzleOffset;
    }

    public void UpgradeWeapon()
    {
        Debug.Log("WeaponAutoAim LevelUP");
        //check level

        if (currentLevel < maxLevel)
        {
            damageFactor = levelParams[currentLevel].damageFactor;
            maxCollideCount = levelParams[currentLevel].maxColliderCount;
            projectilePerRound = levelParams[currentLevel].projectilePerRound;

            currentLevel++;
        }
        if (currentLevel > 0 && !muzzle.gameObject.activeSelf)
        {
            muzzle.gameObject.SetActive(true);
        }
    }

    [System.Serializable]
    public struct WeaponAutoAim_Param
    {
        public float damageFactor;
        public int maxColliderCount;
        public int projectilePerRound;
    }
}

