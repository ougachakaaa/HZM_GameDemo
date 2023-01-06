using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
}

public class EnemyController : LivingEntity
{
    //state
    EnemyState currentState;

    [Header("Attack Behavior")]
    //path finding
    NavMeshAgent enemyAgent;
    public float updateTargetRate = 0.2f;
    CharacterController playerCharacterController;
    
    public Transform playerTransform;

    //public Color attackColor; 
    
    public float attackDistanceThreshold = 1.5f;
    float enemyPlayerDistanceSqr;
    [SerializeField]float attackDuration;
    public float AttackDuration 
    {
        get { return attackDuration > 1 / attackSpeed ? attackDuration : 1 / attackSpeed;  }
        set { attackDuration = value;  }
    }
    float nextAttackTime = 0f;
    [Range(1f,5f)]public float attackSpeed;

    float playerCollisionRadius;
    float enemyCollisionRadius;
    [SerializeField] float attackDepth;

    [SerializeField] float knockBackDistance;


    //hit
    [Header("Take Damage")]
    Material thisMaterial;
    Color originalColor;
    Vector3 originalScale;
    public Color hitColor;
    public float hitScaleFactor = 1.2f;
    public float hitBackwardForce = 4f;
    public float HitEffectDuration = 0.3f;
    public float hitEffectSmoothness = 50;

    //spawn
    EnemySpawner _spawner;


    
    protected override void Start()
    {
        //initializing living entity 
        base.Start();

        currentState = EnemyState.Chasing;

        //initializing hit effect
        thisMaterial = GetComponentInChildren<MeshRenderer>().material;
        originalColor = thisMaterial.color;
        originalScale = transform.localScale;

        //navigation
        enemyAgent = GetComponent<NavMeshAgent>();
        _spawner = FindObjectOfType<EnemySpawner>();
        playerTransform = FindObjectOfType<MowingController>().transform;
        playerCharacterController = playerTransform.GetComponent<CharacterController>();
        playerCollisionRadius = playerCharacterController.radius;
        enemyCollisionRadius = GetComponent<CapsuleCollider>().radius;

        enemyAgent.stoppingDistance = attackDistanceThreshold + enemyCollisionRadius + playerCollisionRadius - attackDepth;

        StartCoroutine(UpdateTargetPosition());
    }
    private void Update()
    {
        AttackCheck();
    }

    public IEnumerator UpdateTargetPosition()
    {
        while (playerTransform != null && !isDead)
        {
            if (currentState == EnemyState.Chasing)
            {
                Vector3 enemyTarget = playerTransform.position;
                enemyTarget.y = 0;
                enemyAgent.destination = enemyTarget;

            }
            yield return new WaitForSeconds(updateTargetRate);
        }

    }

    //Attack
    void AttackCheck()
    {
        if (Time.time > nextAttackTime)
        {
            enemyPlayerDistanceSqr = (playerTransform.position - transform.position).sqrMagnitude;
            if (enemyPlayerDistanceSqr <= Mathf.Pow( (attackDistanceThreshold+enemyCollisionRadius+playerCollisionRadius),2))
            {
                StartCoroutine(Attack());
                nextAttackTime = Time.time + AttackDuration;
            }
        }
    }
    IEnumerator Attack()
    {
        currentState = EnemyState.Attacking;
        enemyAgent.enabled = false;
        
        Vector3 originalPos = transform.position;
        Vector3 playerPos = playerTransform.position;
        Vector3 attackDir = (playerPos - originalPos).normalized * (attackDistanceThreshold);
        Vector3 targetPos = originalPos + attackDir;
        float percent = 0f;
        while (percent <= 1)
        {
            float interpolation = -4*Mathf.Pow((percent - 0.5f),2) + 1;
            transform.position = Vector3.Lerp(originalPos, targetPos, interpolation);

            percent += Time.deltaTime*attackSpeed;
            yield return null;
        }

        transform.position = originalPos;
        currentState = EnemyState.Chasing;
        enemyAgent.enabled = true;
    }
    //attack effect
/*    public IEnumerator KnockBack(Transform target, Vector3 dir,float distance)
    {

        yield return null;
        float duration = 0.5f;
        float lerpTime = 25f;
        WaitForSeconds interval = new WaitForSeconds(duration / lerpTime);
        float sharpness = 0.15f;
        Vector3 targetPos = target.position + dir.normalized * distance;

        Actions.OnPlayerAttacked.Invoke(PlayerState.UnderAttack);
        playerCharacterController.velocity +

        for (int i = 0; i <= lerpTime; i++)
        {
            target.position = Vector3.Lerp(target.position, targetPos, sharpness);
            yield return interval;
        }

        Actions.OnPlayerAttacked.Invoke(PlayerState.Moving);
    }*/


    //take damage
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        enemyAgent.velocity = -hitBackwardForce*transform.forward;
        TriggerEffect(hitColor,hitScaleFactor);
        StartCoroutine(ResetEffect(hitColor, hitScaleFactor,HitEffectDuration, hitEffectSmoothness));


    }
    void TriggerEffect(Color color,float scaleFactor)
    {
        thisMaterial.color = color;
        gameObject.transform.localScale = originalScale * scaleFactor;

    }
    IEnumerator ResetEffect (Color color, float scaleFactor,float duration, float lerpCount)
    {

        WaitForSeconds lerpDuration = new WaitForSeconds( duration / lerpCount);
        float lerpT = 1 / lerpCount;
        Vector3 targetScale = originalScale * scaleFactor;
        for (int i = 0; i < lerpCount; i++)
        {
            thisMaterial.color = Color.Lerp(color, originalColor, lerpT*i);
            gameObject.transform.localScale = Vector3.Lerp(targetScale, originalScale, lerpT*i);
            yield return lerpDuration;
        }
    }

    //die
    protected override void Die()
    {
        base.Die();
        _spawner.enmeyList.Remove(gameObject.transform);
        Destroy(gameObject);
        
    }

}
