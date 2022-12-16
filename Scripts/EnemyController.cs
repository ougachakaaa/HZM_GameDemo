using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent enemyAgent;
    public Transform _playerCharactor;

    //hit
    Material thisMaterial;
    Color originalColor;
    public float hitEffectFadeSharpeness = 0.1f;
    public Color hitColor;
    public float hitBackwardForce = 4f;
    public float colorChangeTimer;
    public float colorChangeDuration = 0.3f;
    private void Awake()
    {
        thisMaterial = GetComponentInChildren<MeshRenderer>().material;
        originalColor = thisMaterial.color;
        enemyAgent = GetComponent<NavMeshAgent>();
        _playerCharactor = FindObjectOfType<MowingController>().transform;
    }
    private void Update()
    {
        enemyAgent.destination = _playerCharactor.position;
        if (colorChangeTimer < colorChangeDuration)
        {
            //reset hit effect
            thisMaterial.color = Color.Lerp(thisMaterial.color,originalColor, hitEffectFadeSharpeness);
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale,new Vector3(1f, 1f, 1f), hitEffectFadeSharpeness);
        }
    }
    public void BeingHit()
    {
        colorChangeTimer = 0;
        enemyAgent.velocity = -hitBackwardForce*transform.forward;

        //trigger hit effects
        thisMaterial.color = hitColor;
        gameObject.transform.localScale = new Vector3(1.2f,1.2f,1.2f);

        
    }
}
