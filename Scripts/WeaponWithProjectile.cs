using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponWithProjectile : MonoBehaviour
{
    public MowingController _controller;

    [Header("Weapon Info")]
    public int weaponID;
    public string weaponName;
    public GameObject weaponIndicator;

    //damage
    public float damageFactor;
    public float existingTime;

    //locate enemy
    [SerializeField]EnemySpawner _enmeySpawner;
    Vector3 targetPos;
    Vector3 targetDir;

    //fire
    [Header("Projectile")]
    [SerializeField] Projectile _projectile;
    [SerializeField] float _speed;
    [SerializeField] float projectileDuration =0.08f;
    [SerializeField] int projectilePerRound =3;
    [SerializeField] float RoundDuration =1f;
    public int projectileMaxCollideTime =3;
    float _muzzleHeight;

    private void Awake()
    {
        _muzzleHeight = _controller.muzzleHeight;
        gameObject.name = weaponName;
        if(_projectile != null)
        {
            StartCoroutine(FireProjectile());
        }
    }
    private void Update()
    {
        if(weaponIndicator != null)
        {
            weaponIndicator.transform.position = GetMuzzlePos();
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
    public IEnumerator FireProjectile()
    {
        while (true)
        {
            int projectileCountThisRound = 0;
            while(projectileCountThisRound<projectilePerRound && _enmeySpawner.enmeyList != null && _enmeySpawner.enmeyList.Count > 0)
            {
                Instantiate(_projectile, GetMuzzlePos(), Quaternion.LookRotation(targetPos - transform.position));
                _projectile._weapon = this;
                projectileCountThisRound++;
                yield return new WaitForSeconds(projectileDuration);
            }
            yield return new WaitForSeconds(RoundDuration);
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

