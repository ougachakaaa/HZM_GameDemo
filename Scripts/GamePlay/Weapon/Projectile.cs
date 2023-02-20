using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Projectile : MonoBehaviour
{
    public LayerMask projectileCollisionMask;

    //collide count (numbers of the enemy that this projectile can hit before it is detroyed)
    float _maxCollideCount;
    float _currentCollideCount;
    float _existingTime;
    float timer;

    TrajectoryType _trajectory;
    float _speed;
    float _damage;

    [Header("VFX")]
    public static Transform VfxHolder;
    public ParticleSystem hitVFX;
    public TrailRenderer trail;

    protected Action<Projectile> ReleaseThisProjectile;

    private void OnDisable()
    {

    }
    private void OnEnable()
    {

        Collider[] initialColliders = Physics.OverlapSphere(transform.position, 0.5f, projectileCollisionMask, QueryTriggerInteraction.Collide);
        if (initialColliders.Length > 0)
        {
            HitObject(initialColliders[0]);
        }


    }
    private void Awake()
    {
        if (VfxHolder == null)
        {
            VfxHolder = new GameObject("HitVFXHolder").transform;
        }
        trail = GetComponentInChildren<TrailRenderer>();
        hitVFX = Instantiate(hitVFX, VfxHolder);

    }
    private void Start()
    {
        //Debug.Log("Start");
    }
    
    private void Update()
    {
        transform.Translate(transform.forward * _speed * Time.deltaTime,Space.World);
        Debug.DrawRay(transform.position,transform.forward);
        float moveDistance = _speed * Time.deltaTime;
        HitCheck(moveDistance/2 + transform.localScale.z);

        //check for existing time;
        if (Time.time > timer+_existingTime)
        {
            ReleaseThisProjectile?.Invoke(this);
        }
    }

    public void InitializeProjectile(Weapon _weapon)
    {
        _trajectory = _weapon.trajectory;
        _maxCollideCount = _weapon.maxCollideCount;
        _existingTime = _weapon.existingTime;
        _damage = _weapon.fireDamage;
        _speed = _weapon.speed;

        _currentCollideCount = 0;
        timer = Time.time;

        if (ReleaseThisProjectile == null)
        {
            Debug.Log("register release func here");
            ReleaseThisProjectile += _weapon.projectilePool.Release;
            ReleaseThisProjectile += TriggerHitVFX;
        }
    }
    public void HitCheck(float checkDistance)
    {
        Ray ray = new Ray(transform.position-transform.forward*0.3f, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, checkDistance, projectileCollisionMask, QueryTriggerInteraction.Collide))
        {
            HitObject(hit.collider);
        }
    }
    public void HitObject(Collider c)
    {
        IDamagable damagableObject = c.GetComponent<IDamagable>();
        if (damagableObject != null)
        {
            damagableObject.TakeDamage(_damage);

            _currentCollideCount++;
            if (_currentCollideCount >= _maxCollideCount)
            {
                ReleaseThisProjectile?.Invoke(this);
            }
        }
        else
        {
            ReleaseThisProjectile?.Invoke(this);
        }
        


    }
    void TriggerHitVFX(Projectile p)
    {

        hitVFX.transform.position = transform.position;
        hitVFX.Play();
    }

    public Vector3 CalculteSpeed(TrajectoryType _trajectory)
    {
        Vector3 dir;
        switch (_trajectory)
        {
            case TrajectoryType.Forward:
                dir = transform.forward;
                break;
            default:
                dir = transform.forward;
                break;
        }
        return dir;
    }

}
