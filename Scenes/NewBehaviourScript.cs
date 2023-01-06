using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public int count;
    public int coroutineCount;
    public Transform obj;
    public Transform target;
    IEnumerator countUp;
    IEnumerator countUp2;
    Coroutine countUpCor;
    // Start is called before the first frame update
    void Start()
    {
        count = 1;
        coroutineCount = 0;
        countUp = ConutUp(count);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StopCoroutine(countUp);
            StartCoroutine(countUp);
            
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            StopAllCoroutines();
        }
        /*        obj.rotation = Quaternion.LookRotation(target.position-obj.position);
                Debug.DrawRay(obj.position, obj.forward,Color.cyan);
                transform.Translate(Vector3.forward*Time.deltaTime,Space.Self);*/
    }
    IEnumerator ConutUp(int increasement)
    {
        Debug.Log("Start: " + ++coroutineCount);
        while (true)
        {
            count+=increasement;
            tmp.text = count.ToString();
            Debug.Log(count);
            yield return new WaitForSeconds(1f);

        }
    }

}
