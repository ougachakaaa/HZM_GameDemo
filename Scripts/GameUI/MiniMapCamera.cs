using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    public enum MiniMapCamMode
    {
        FollowPlayer,
        FocusOnCenter,
    }
    [SerializeField] Vector3 offset;
    [SerializeField] Transform playerTransform;
    [SerializeField] MapManager mapManager;
    [SerializeField] Camera thisCam;
    [SerializeField] float camProjectionSizeOffset;
    [SerializeField] MiniMapCamMode camMode;

    private void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
        playerTransform = FindObjectOfType<PlayerCharacterController>().transform;
        thisCam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        switch (camMode)
        {
            case MiniMapCamMode.FollowPlayer:
                transform.position = playerTransform.position+offset;
                thisCam.orthographicSize = mapManager.mapUnitSize * 5;
                break;

            case MiniMapCamMode.FocusOnCenter:
                transform.position = Vector3.zero + offset;
                int projectionSize = Mathf.Max(mapManager.currentMap.mapSize.x, mapManager.currentMap.mapSize.y);
                thisCam.orthographicSize = mapManager.mapUnitSize*projectionSize/2+camProjectionSizeOffset;
                break;

            default:
                break;
        }
    }
}
