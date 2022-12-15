using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent enemyAgent;
    public Transform _playerCharactor;
    private void Awake()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        _playerCharactor = FindObjectOfType<MowingController>().transform;
    }
    private void Update()
    {
        enemyAgent.destination = _playerCharactor.position;
    }
}
