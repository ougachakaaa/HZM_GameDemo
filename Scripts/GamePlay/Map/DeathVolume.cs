using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathVolume : MonoBehaviour
{
    public BoxCollider deathTrigger;
    public LayerMask targetLayer;
    MapManager mapManager;
    Vector3 scale;
    // Start is called before the first frame update
    void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
        scale = new Vector3(mapManager.maxMapSize.x, 1, mapManager.maxMapSize.y);
        transform.localScale = scale *mapManager.mapUnitSize/10;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerLivingEntity>()?.Die();
            Debug.Log("Die");
        }
    }
}
