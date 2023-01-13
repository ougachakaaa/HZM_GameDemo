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
    //Player transform
    public Transform playerTransform;
    PlayerLivingEntity _playerLivingEntity;


    [Header("Attack Behavior")]
    //path finding
    NavMeshAgent enemyAgent;
    public float updateTargetRate = 0.2f;

    //attack parameters
    public float attackDistanceThreshold = 1.5f;
    [SerializeField]float attackDuration;
    public float AttackDuration 
    {
        get { return attackDuration > 1 / attackSpeed ? attackDuration : 1 / attackSpeed;  }
        set { attackDuration = value;  }
    }
    [Range(1f,5f)]public float attackSpeed;
    [SerializeField] float attackDepth;
    [SerializeField] float knockBackDistance;

    float enemyPlayerDistanceSqr;
    float nextAttackTime = 0f;
    float playerCollisionRadius;
    float enemyCollisionRadius;
    bool hasTriggerDamage;


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

        thisMaterial = GetComponentInChildren<MeshRenderer>().material;
        originalColor = thisMaterial.color;
        originalScale = transform.localScale;

        InitEnemyController();

    }

    public void InitEnemyController()
    {
        InitializeLivingEntity();

        currentState = EnemyState.Chasing;

        //navigation
        enemyAgent = GetComponent<NavMeshAgent>();
        _spawner = FindObjectOfType<EnemySpawner>();
        playerTransform = FindObjectOfType<MowingController>().transform;
        enemyAgent.ActivateCurrentOffMeshLink(true);

        playerCollisionRadius = playerTransform.GetComponent<CharacterController>().radius;
        enemyCollisionRadius = GetComponent<CapsuleCollider>().radius;
        enemyAgent.stoppingDistance = attackDistanceThreshold + enemyCollisionRadius + playerCollisionRadius - attackDepth;

        _playerLivingEntity = playerTransform.GetComponent<PlayerLivingEntity>();

        StartCoroutine(UpdateTargetPosition());
    }
    public void ResetEnemyController()
    {
        StopAllCoroutines();
        thisMaterial.color = originalColor;
        transform.localScale = originalScale;
    }

    private void Update()
    {
        if (!_playerLivingEntity.isDead)
        {
            AttackCheck();
        }
    }

    public IEnumerator UpdateTargetPosition()
    {
        while (playerTransform != null && !isDead && !Actions.IsPlayerDead)
        {
            if (currentState == EnemyState.Chasing && enemyAgent.isOnNavMesh)
            {
                Vector3 enemyTarget = playerTransform.position;
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

            if (percent >0.5f && hasTriggerDamage == false)
            {
                hasTriggerDamage = true;
                Actions.OnEnemyAttack(attackPoint);
            }

            percent += Time.deltaTime*attackSpeed;
            yield return null;
        }

        hasTriggerDamage = false;
        transform.position = originalPos;
        currentState = EnemyState.Chasing;
        enemyAgent.enabled = true;
    }

    //take damage
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        enemyAgent.velocity = -hitBackwardForce*transform.forward;
        if (!isDead)
        {
            TriggerEffect(hitColor,hitScaleFactor);
            StartCoroutine(ResetEffect(hitColor, hitScaleFactor,HitEffectDuration, hitEffectSmoothness));
        }



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
        for (int i = 0; i <= lerpCount; i++)
        {
            thisMaterial.color = Color.Lerp(color, originalColor, lerpT*i);
            gameObject.transform.localScale = Vector3.Lerp(targetScale, originalScale, lerpT*i);
            yield return lerpDuration;
        }
    }

    //die
    public override void Die()
    {
        base.Die();
        Actions.OnEnemyDie.Invoke(this);
        //StartCoroutine(DieEffect(0.5f));

    }
    IEnumerator DieEffect(float time)
    {
        Color transparent = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        float division = 25f;
        WaitForSeconds wait = new WaitForSeconds(time / division);
        for (int i = 0; i <= division; i++)
        {
            thisMaterial.color = Color.Lerp(originalColor, transparent, i / division);
            yield return wait;
        }


        Debug.Log("invoke onEnemyDie here");
        yield return null;

    }

}
