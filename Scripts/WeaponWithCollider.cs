using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponWithCollider : MonoBehaviour
{
    public MowingController _controller;

    [Header("Weapon Info")]
    public int weaponID;
    public string weaponName;
    public GameObject weaponIndicator;
    //damage
    public float damageFactor;
    public float CharacterAttackPoint
    {
        get
        {
            if (_controller != null)
                return _controller.attackPoint;
            else
            {
                Debug.Log("can't find controller, attack point set to default : 100");
                return 100;
            }

        }
    }

    //fire
    [Header("Collider")]
    public WeaponCollider collider;
    public List<WeaponCollider> _colliders = new List<WeaponCollider>();
    public float _angularSpeed;
    [SerializeField] int _colliderCount;
    [SerializeField] float _radius;
    float muzzleHeight;


    public float Radius
    {
        get { return _radius; }
        set
        {
            _radius = value < 2 ? 2 : value;
        }
    }
    public int ColliderCount
    {
        get { return _colliderCount; }
        set { _colliderCount = value < 2 ? 2 : value; }
    }

    private void Awake()
    {
        muzzleHeight = _controller.muzzleHeight;
        gameObject.name = weaponName;
    }
    private void Update()
    {
        transform.forward = Vector3.forward;
        if (_controller._input.shoot)
        {
            UpdateCollider(_colliderCount);
            _controller._input.shoot = false;
        }
    }
    public void UpdateCollider(int count)
    {
        if (_colliders.Count>0)
        {
            foreach(WeaponCollider c in _colliders)
            {
                Debug.Log("dlear here");
                Destroy(c.gameObject);
            }
            _colliders.Clear();
        }
        collider._weapon = this;
        for(int i = 1; i <= count; i++)
        {
            Debug.Log("updateCollider here");
            float angularInterval = Mathf.PI*2 / _colliderCount;
            Vector3 _spawnPos = new Vector3(_radius * Mathf.Cos(angularInterval*i),muzzleHeight, _radius * Mathf.Sin(angularInterval * i))+_controller.transform.position;
            _colliders.Add(Instantiate<WeaponCollider>(collider, _spawnPos, Quaternion.identity, transform));
            
            
        }
    }





}
