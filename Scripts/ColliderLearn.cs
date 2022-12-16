using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderLearn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
    }
}
