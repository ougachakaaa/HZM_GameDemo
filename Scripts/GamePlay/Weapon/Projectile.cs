using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    public WeaponBase _weapon;
    public LayerMask projectileCollisionMask;

    //collide count (numbers of the enemy that this projectile can hit before it is detroyed)
    float _maxCollideCount;
    float _currentCollideCount;
    float _existingTime;

    TrajectoryType _trajectory;
    float _speed;
    float _damage;

    [Header("HitVFX")]
    public GameObject hitVFX;

    
    private void Awake()
    {
        Collider[] initialColliders = Physics.OverlapSphere(transform.position,0.5f,projectileCollisionMask,QueryTriggerInteraction.Collide);
        if (initialColliders.Length > 0)
        {
            OnHitObject(initialColliders[0]);
        }
    }
/*    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 1f);
    }*/
    private void Update()
    {
        transform.Translate(transform.forward * _speed * Time.deltaTime,Space.World);
        Debug.DrawRay(transform.position,transform.forward);
        float moveDistance = _speed * Time.deltaTime;
        HitCheck(moveDistance/2 + transform.localScale.z);
    }

    public void InitializeProjectile(WeaponBase _weapon)
    {
        _trajectory = _weapon.trajectory;
        _maxCollideCount = _weapon.maxCollideCount;
        _existingTime = _weapon.existingTime;
        _damage = _weapon.damage;
        _speed = _weapon.speed;

        _currentCollideCount = 0;

        Destroy(gameObject, _existingTime);
    }
    public void HitCheck(float checkDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, checkDistance, projectileCollisionMask, QueryTriggerInteraction.Collide))
        {
            Debug.Log(hit.collider);
            OnHitObject(hit.collider);
        }
    }
    public void OnHitObject(Collider c)
    {
        IDamagable damagableObject = c.GetComponent<IDamagable>();
        PlayHitVFX(transform.position);
        if (damagableObject != null)
        {
            damagableObject.TakeDamage(_damage);

            _currentCollideCount++;
            if (_currentCollideCount >= _maxCollideCount)
                Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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

    private void PlayHitVFX(Vector3 _hitPos)
    {
        GameObject _hit = Instantiate(hitVFX, _hitPos, Quaternion.identity);
        Destroy(_hit, 0.2f);

    }
}
