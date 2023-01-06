using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public enum TrajectoryType
{
    Forward,
    Tracing,
}
[System.Serializable]
public class WeaponBase:MonoBehaviour
{
    //weapon info
    [Header("Weapon Info")]
    public string weaponName;
    public int weaponID;

    [Header("Damage")]
    //damage
    public float damageFactor;
    public float damage;

    [Header("Projectile")]
    public Projectile projectilePrefabs;
    public float speed;
    public float existingTime;
    public int maxCollideCount;
    public TrajectoryType trajectory;

    [Header("Fire")]
    public int projectilePerRound = 3;
    public float fireDuration = 0.1f;
    public float roundDuration=1f;
    public float muzzleHeight;
    Vector3 muzzlePos;
    Vector3 targetPos;
    public EnemySpawner _spawner;
    public MowingController _player;
    public Transform muzzleIndicator;
    public Transform debugProject;
    public Transform debugTarget;
    Transform projectilePool;

    private void Start()
    {
        projectilePool = GameObject.Find("ProjectilePool").transform;
        StartCoroutine(Fire());
    }
    private void Update()
    {

    }
    public IEnumerator Fire()
    {
        while (true)
        {
            for (int i = 0; i < projectilePerRound; i++)
            {
                targetPos = _spawner.FindClosestEnemy();
                muzzlePos = GetMuzzleOffset(targetPos) + transform.position;
                muzzleIndicator.position = GetMuzzleOffset(targetPos) + transform.position;

                if (_spawner.enmeyList.Count > 0)
                {
                    damage = damageFactor * _player.attackPoint;
                    Projectile thisProjectile = Instantiate<Projectile>(projectilePrefabs,transform.position+new Vector3(0, muzzleHeight,0) ,Quaternion.LookRotation(targetPos-_player.transform.position),projectilePool);
                    thisProjectile.InitializeProjectile(this);

                }
                yield return new WaitForSeconds(fireDuration);
            }
            yield return new WaitForSeconds(roundDuration);
        }

    }
    public Vector3 GetMuzzleOffset(Vector3 target)
    {
        Vector3 muzzleOffset = Vector3.ProjectOnPlane(target - transform.position,transform.up).normalized/2;
        muzzleOffset.y = muzzleHeight;
        return muzzleOffset;
        
    }


}
