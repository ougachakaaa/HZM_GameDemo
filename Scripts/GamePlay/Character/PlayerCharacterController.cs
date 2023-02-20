using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;
using Cinemachine;
using StarterAssets;
public class PlayerCharacterController : MonoBehaviour
{
    [Header("Input")]
    public StarterAssetsInputs _input;
    public CharacterController _controller;
    public ThirdPersonController thirdPersonController;

    //aiming
    [Header("Aim")]
    [SerializeField] Image _crosshair;
    [SerializeField] CinemachineVirtualCamera _aimCamera;
    [SerializeField] LayerMask aimLayerMask = new LayerMask();
    public Transform targetIndicator;
    public float muzzleHeight;
    Vector3 targetPos;
    RaycastHit rayHit;
    public bool canShoot;

    [SerializeField] Rig aimRig;
    [SerializeField] Rig WeaponRig;
    Animator animator;
    float shootAnimationWeight;

    [Header("Dash")]
    [SerializeField] bool canDash;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashLastTime;
    [SerializeField] [Range(0,1f)]float endSpeedPercentage;
    [SerializeField] ParticleSystem dashVFX;
    float dashTimer;
    float dashCD;

    private void Start()
    {
        animator = GetComponent<Animator>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        _controller = GetComponent<CharacterController>();

        dashTimer = 0f;
    }

    void Update()
    {
        if (!_input.pause)
        {
            _crosshair.gameObject.SetActive(_input.aim);
            _aimCamera.gameObject.SetActive(_input.aim);
            PrepareForShoot(_input.aim || _input.shoot);
    
            targetPos = GetAimPoint(_input.aim);
            targetIndicator.position = targetPos;
            //GetAimingRotation();
            if (_input.aim)
            {
                Vector3 targetDir = Vector3.ProjectOnPlane(targetPos - transform.position,Vector3.up);
                transform.forward = Vector3.Lerp(transform.forward, targetDir, thirdPersonController.RotationSmoothTime*0.3f);
            }

            if (_input.dash && !_input.aim)
            {
                if (!thirdPersonController.Grounded && canDash)
                {
                    dashVFX.Play();
                    StartCoroutine(Dash());
                }
            }
            if (thirdPersonController.Grounded)
            {
                canDash = true;
            }
            else
            {
                _input.aim = false;
            }
        }
    }
    
    public void PrepareForShoot(bool isShooting)
    {
        if (isShooting)
        {
            shootAnimationWeight += Time.deltaTime*5;
        }
        else
        {
            shootAnimationWeight -= Time.deltaTime * 5;
        }
        shootAnimationWeight = Mathf.Clamp(shootAnimationWeight, 0f, 1f);
        animator.SetLayerWeight(1, shootAnimationWeight);
        aimRig.weight = shootAnimationWeight;
        //WeaponRig.weight = 1 - shootAnimationWeight;

        canShoot = (shootAnimationWeight >= 0.99f) && _input.shoot;
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
            Vector3 startPos = transform.position + Vector3.up * muzzleHeight;
            _ray = new Ray(startPos, transform.forward);
        }
        if (Physics.Raycast(_ray, out rayHit ,200f, aimLayerMask))
        {
            _targetPoint = rayHit.point;
        }
        else
        {
            _targetPoint = _ray.GetPoint(200f);
        }
        return _targetPoint;
    }

    IEnumerator Dash()
    {
        thirdPersonController.SetCharacterState(PlayerState.Dash);
        canDash = false;
        Vector3 beginVelocity = transform.forward * Time.deltaTime * dashSpeed;
        Vector3 endVelocity = beginVelocity*endSpeedPercentage;
        float percentage = 0f;
        while (percentage <= 1f)
        {
            percentage = dashTimer / dashLastTime;
            Vector3 dashV = Vector3.Lerp(beginVelocity,endVelocity,percentage);
            _controller.Move(dashV);
            dashTimer += Time.deltaTime;
            yield return null;
        }
        thirdPersonController.SetCharacterState(PlayerState.Move);
        dashTimer = 0f;
        yield return null;
    }



}
