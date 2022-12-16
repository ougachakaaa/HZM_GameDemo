using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    public WeaponWithCollider _weapon;

    float _damage;
    Rigidbody _colliderRb;
    public GameObject hitVFX;
    // Start is called before the first frame update
    private void Awake()
    {
        _colliderRb = GetComponent<Rigidbody>();
        UpdateProjectile(_weapon);

    }

    // Update is called once per framew
    void Update()
    {
        transform.RotateAround(_weapon.transform.position,Vector3.up,Mathf.Rad2Deg*_weapon._angularSpeed*Time.deltaTime);
    }
    public void UpdateProjectile(WeaponWithCollider _weapon)
    {
        _damage = _weapon.damageFactor * _weapon._controller.attackPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<LiveSystem>() != null)
        {
            LiveSystem _targetLiveSystem = other.gameObject.GetComponent<LiveSystem>();
            _targetLiveSystem.TakeDamage(_damage);
        }
    }
}
