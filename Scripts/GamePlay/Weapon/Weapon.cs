using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public enum TrajectoryType
{
    Forward,
    Tracing,
}
[System.Serializable]
public class Weapon:MonoBehaviour
{
    //weapon info
    [Header("Weapon Info")]
    public string weaponName;
    public int weaponID;

    [Header("Damage")]
    //damage
    public float damageFactor;
    [HideInInspector]
    public float fireDamage;

    [Header("Projectile")]
    public Projectile projectilePrefabs;
    public float speed;
    public float existingTime;
    public int maxCollideCount;
    public TrajectoryType trajectory;
    //projectile pool
    public ObjectPool<Projectile> projectilePool;
    protected int defaultCapaticy = 40;
    protected int maxSize = 70;


    [Header("Fire")]
    protected float fireTimer;
    public float fireDuration = 0.2f;
    public PlayerCharacterController _playerCharacterController;
    public PlayerLivingEntity _playerLivingEntity;
    protected Transform muzzle;
    protected Transform projectileHolder;

    //level stuff
    protected int maxLevel;
    public int currentLevel;

    private void OnDisable()
    {

    }

    protected virtual void Start()
    {
        _playerLivingEntity = FindObjectOfType<PlayerLivingEntity>();
        _playerCharacterController = FindObjectOfType<PlayerCharacterController>();
        muzzle = transform.Find("Muzzle");

        projectileHolder = new GameObject($"{weaponName}:ProjectileHolder").transform;

        projectilePool = new ObjectPool<Projectile>(OnCreatePoolItem, OnGetPoolItem, OnReleasePoolItem, OnDestroyPoolItem,true, defaultCapaticy, maxSize);

        fireTimer = 0f;
    }
    protected void FireProjectile(Projectile p, Vector3 position,Quaternion rotation)
    {
        fireDamage = damageFactor * _playerLivingEntity.attackPoint;

        p.transform.rotation = rotation;
        p.InitializeProjectile(this);
    }


    #region projectile pool stuff
    //enmeyPool
    private void OnDestroyPoolItem(Projectile obj)
    {
        Destroy(obj);
    }
    private void OnReleasePoolItem(Projectile obj)
    {
        obj.trail.Clear();
        obj.gameObject.SetActive(false);
    }

    private void OnGetPoolItem(Projectile obj)
    {
        obj.transform.position = muzzle.position;
        obj.gameObject.SetActive(true);
    }
    private Projectile OnCreatePoolItem()
    {
        Projectile thisProjectile = Instantiate<Projectile>(projectilePrefabs, projectileHolder);
        thisProjectile.transform.parent = projectileHolder;
        return thisProjectile;
    }
    #endregion

}
