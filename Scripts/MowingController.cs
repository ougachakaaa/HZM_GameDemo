using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using StarterAssets;
public class MowingController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] StarterAssetsInputs _input;

    //aiming
    [Header("Aim")]
    [SerializeField] Image _crosshair;
    [SerializeField] CinemachineVirtualCamera _aimCamera;
    [SerializeField] GameObject muzzle;
    //[SerializeField] GameObject target;
    [SerializeField] LayerMask aimLayerMask = new LayerMask();
    [SerializeField] [Range(0,0.1f)]float rotateSharpness = 0.1f;
    Vector3 targetPoint;
    RaycastHit rayHit;
    Vector3 aimDirection;

    //Weapon
    [Header("Weapon")]
    public Transform WeaponSet;
    public float attackPoint;
    [SerializeField] GameObject projectile;
    [SerializeField] [Range(0.1f,10f)]float shootingRatePerSec;
    float shootingDuration;
    float shootingTimer;

    private void Awake()
    {
        WeaponSet = GameObject.Find("WeaponSet").transform;
    }

    void Update()
    {
        shootingDuration = 1 / shootingRatePerSec;
        if (_input.aim)
        {
            _crosshair.gameObject.SetActive(true);
            _aimCamera.gameObject.SetActive(true);
        }
        else
        {
            _crosshair.gameObject.SetActive(false);
            _aimCamera.gameObject.SetActive(false);
        }
        targetPoint = GetAimPoint(_input.aim);
        GetAimingRotation();
        //target.transform.position = targetPoint;
        if(shootingTimer >= shootingDuration)
        {
            Shooting();
        }
        shootingTimer += Time.deltaTime;
    }
    Vector3 GetAimPoint(bool isAiming)
    {
        Ray _ray;
        Vector3 _targetPoint;
        if (isAiming)
        {
            _ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width/2,Screen.height/2));
        }
        else
        {
            _ray = new Ray(muzzle.transform.position,muzzle.transform.forward);
        }
        if (Physics.Raycast(_ray, out rayHit ,50f, aimLayerMask))
        {
            _targetPoint = rayHit.point;
        }
        else
        {
            _targetPoint = _ray.GetPoint(20f);
        }
        return _targetPoint;
    }
    public void GetAimingRotation()
    {
        //indicate aim direction
        aimDirection = targetPoint - muzzle.transform.position;

        //rotate character to the projection of aim direction on the xzPlan
        aimDirection = Vector3.ProjectOnPlane(aimDirection,transform.up).normalized;
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, rotateSharpness);

    }
    void Shooting()
    {

        if (_input.shoot)
        {
            shootingTimer = 0f;
        }
    }
}
