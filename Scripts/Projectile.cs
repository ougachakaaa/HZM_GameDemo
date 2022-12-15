using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Weapon _weapon;
    float _existingTime;

    float _damage;
    Rigidbody projectileRB;

    public GameObject hitVFX;

    // Start is called before the first frame update

    private void Awake()
    {
        _weapon = GameObject.FindObjectOfType<Weapon>();
        projectileRB = GetComponent<Rigidbody>();
        UpdateProjectile(_weapon);
        if(_existingTime > 2f)
        {
            Destroy(gameObject,_existingTime);
        }
        projectileRB.velocity = _weapon.CalculateProjectileVelocity(_weapon.name);
    }

    // Update is called once per frame

    public void UpdateProjectile(Weapon _weapon)
    {
        _existingTime = _weapon.existingTime;
        _damage = _weapon.damageFactor * _weapon.CharacterAttackPoint;
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.GetComponent<LiveSystem>() != null)
        {
            LiveSystem _targetLiveSystem = collision.gameObject.GetComponent<LiveSystem>();
            _targetLiveSystem.TakeDamage(_damage);
        }
        else
        {
        }

        GameObject _hit = Instantiate(hitVFX, transform.position, Quaternion.identity);
        Destroy(_hit,0.2f);
        Destroy(gameObject);
    }
}
