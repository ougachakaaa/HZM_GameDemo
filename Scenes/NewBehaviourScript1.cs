using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript1 : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {

            StartCoroutine(Log());

        }
    }
    IEnumerator Log()
    {
        while (true)
        {
            Debug.Log("-----");
            yield return new WaitForSeconds(0.5f);

        }
    }
}
