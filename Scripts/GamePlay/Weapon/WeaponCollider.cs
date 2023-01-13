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
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(_weapon.transform.position,Vector3.up,Mathf.Rad2Deg*_weapon._angularSpeed*Time.deltaTime);
    }
}
