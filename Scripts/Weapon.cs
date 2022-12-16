using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon : MonoBehaviour
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

    public float existingTime;



    //locate enemy
    [SerializeField]EnemySpawner _enmeySpawner;
    Vector3 targetPos;
    Vector3 targetDir;


    //fire
    [Header("Projectile")]
    [SerializeField] Projectile _projectile;
    [SerializeField] float _speed;
    [SerializeField] float fireDuration =0.5f;
    float _muzzleHeight;
    float fireTimer = 0f;

    private void Awake()
    {
        _muzzleHeight = _controller.muzzleHeight;
        gameObject.name = weaponName;
    }
    private void Update()
    {
        if(weaponIndicator != null)
        {
            weaponIndicator.transform.position = GetMuzzlePos();
        }
        if(_projectile != null)
        {
            FireCheck();
        }
        else
        {
            Debug.Log(weaponName + "don't have projectile");
        }
    }

    public Vector3 FindClosetEnemy(List<Transform> _enemeyList)
    {
        Vector3 closetEnemyPos = new Vector3();
        float closestDistance = Mathf.Infinity;
        foreach(Transform _transform in _enemeyList)
        {
            float thisDistance = Vector3.Distance(_transform.position,_controller.transform.position);
            if(thisDistance < closestDistance)
            {
                closestDistance = thisDistance;
                closetEnemyPos = _transform.position;
            }
        }
        return closetEnemyPos;
    }
    public void FireCheck()
    {
        fireTimer += Time.deltaTime;
        if (_enmeySpawner.enmeyList != null && _enmeySpawner.enmeyList.Count > 0 && fireTimer >= fireDuration)
        {
            //shoot
            Instantiate(_projectile, GetMuzzlePos(), Quaternion.LookRotation(targetPos-transform.position));
            _projectile._weapon = this;
            
            //reset fireTimer
            fireTimer = 0f;
        }
    }

    public Vector3 CalculateProjectileVelocity(string _weaponName)
    {
        Vector3 _projectileVelocity;
        switch (_weaponName)
        {
            case "Kunai":
                {
                    targetPos = FindClosetEnemy(_enmeySpawner.enmeyList);
                    targetDir = (targetPos - _controller.transform.position).normalized;
                    _projectileVelocity = targetDir * _speed;
                    break;
                }
            default:
                {
                    _projectileVelocity = Vector3.up;
                }
                break;


        }
        return _projectileVelocity;
    }
    public Vector3 GetMuzzlePos()
    {
        Vector3 _muzzlePos = CalculateProjectileVelocity(weaponName).normalized/2 + _controller.transform.position;
        _muzzlePos.y = _muzzleHeight+transform.position.y;
        return _muzzlePos;
    }
}

