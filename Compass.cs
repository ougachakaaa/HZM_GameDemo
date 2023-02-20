using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.forward = Vector3.forward;
    }
}
