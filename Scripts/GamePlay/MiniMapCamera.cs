using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    [SerializeField] Vector3 offset = new Vector3(0,10,0);
    [SerializeField] Transform targetTransform;
    private void LateUpdate()
    {
        transform.position = targetTransform.position+offset;

    }
}
