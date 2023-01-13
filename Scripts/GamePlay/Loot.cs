using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [Header("Rotate")]
    public bool isRotating;
    [SerializeField] float rotateSpeed=180f;

    [Header("Float")]
    public bool isFloating;
    [SerializeField] float floatSpeed=1f;
    [SerializeField] float floatRange=1f;

    [Header("Collect")]
    public bool isCollected = false;
    [SerializeField] float flyDistance=1.5f;
    [SerializeField] float flyToDuration = 0.2f;
    [SerializeField] float flyAwayDurtion = 0.1f;

    Transform playerTransform;


    // Start is called before the first frame update
    private void Start()
    {
        SwitchAnimation(true);
    }
    // Update is called once per frame
    void Update()
    {
        Rotating();
        Floating();
    }
    void Rotating()
    {
        if (isRotating)
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
        }
    }
    void Floating()
    {
        if (isFloating)
        {
            float yPos = Mathf.Sin(Time.time*Mathf.PI*floatSpeed) * floatRange;
            transform.position += new Vector3(0, yPos, 0)*Time.deltaTime;
        }
    }
    public void SwitchAnimation(bool state)
    {
        isFloating = state;
        isRotating = state;
    }


    IEnumerator TriggerCollectEffect(Vector3 targetPos)
    {
        isCollected = true;
        Vector3 originalPostion = transform.position;
        Vector3 dir = targetPos - transform.position;
        float elapsedTime = 0;

        while(elapsedTime <= flyAwayDurtion)
        {
            //simply fly away from player
            transform.position = originalPostion - elapsedTime/flyAwayDurtion * dir*flyDistance;
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        elapsedTime = 0;
        Vector3 farthestPos = transform.position;
        float step;

        while (elapsedTime <= flyToDuration)
        {
            step = -Mathf.Cos((elapsedTime / flyToDuration) * Mathf.PI)+1;
            transform.position = Vector3.Lerp(farthestPos,playerTransform.position+Vector3.up,step/2);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        isCollected = false;
        yield return null;

    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            SwitchAnimation(false);
            if (!isCollected)
            {
                StartCoroutine(TriggerCollectEffect(other.transform.position+Vector3.up));
            }
            else
            {
                Actions.OnLootCollected.Invoke(this);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        SwitchAnimation(true);
    }
}
