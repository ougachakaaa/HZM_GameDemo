using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public WeaponWithProjectile _weapon;


    float _maxCollideCount;
    float _currentCollideCount;
    float _existingTime;
    float _damage;
    Rigidbody projectileRB;

    public GameObject hitVFX;

    // Start is called before the first frame update

    private void Awake()
    {
        _currentCollideCount = 0;
        _maxCollideCount = _weapon.projectileMaxCollideTime;
        _existingTime = _weapon.existingTime;
        projectileRB = GetComponent<Rigidbody>();
        UpdateProjectile(_weapon);
        if(_existingTime > 2f)
        {
            Destroy(gameObject,_existingTime);
        }
        projectileRB.velocity = _weapon.CalculateProjectileVelocity(_weapon.name);
    }

    // Update is called once per frame

    public void UpdateProjectile(WeaponWithProjectile _weapon)
    {
        _damage = _weapon.damageFactor * _weapon._controller.attackPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<LiveSystem>() != null)
        {
            PlayHitVFX(transform.position);
            LiveSystem _targetLiveSystem = other.GetComponent<LiveSystem>();
            _targetLiveSystem.TakeDamage(_damage);
            _currentCollideCount++;
            if(_currentCollideCount >= _maxCollideCount)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            PlayHitVFX(transform.position);
            Destroy(gameObject);
        }

    }
    private void PlayHitVFX(Vector3 _hitPos)
    {
        GameObject _hit = Instantiate(hitVFX, _hitPos, Quaternion.identity);
        Destroy(_hit, 0.2f);

    }
}
