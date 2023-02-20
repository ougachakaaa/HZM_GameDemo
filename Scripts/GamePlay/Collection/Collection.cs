using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectionType
{
    Coin=0,
    Diamond=1,
}
public class Collection : MonoBehaviour
{
    public CollectionType collectionType;
    [Header("Rotate")]
    public bool isRotating;
    [SerializeField] protected float rotateSpeed;

    [Header("Float")]
    public bool isFloating;
    [SerializeField] protected float floatSpeed;
    [SerializeField] protected float floatRange;

    [Header("Collect")]
    public bool isCollected;
    [SerializeField] protected float flyDistance;
    [SerializeField] protected float flyToDuration;
    [SerializeField] protected float flyAwayDurtion;

    [SerializeField] protected ParticleSystem VFX;
    public static Transform collectionVFXHolder;

    Transform playerTransform;
    public CollectionManager collectionManager;

    protected void Start()
    {
        VFX = Instantiate(VFX, collectionVFXHolder);
    }

    protected virtual void OnEnable()
    {
        if (collectionManager ==null)
        {
            collectionManager = FindObjectOfType<CollectionManager>();
        }
        if (collectionVFXHolder ==null)
        {
            collectionVFXHolder = new GameObject("CollectionVFXHolder").transform;
        }
        SwitchAnimation(true);
        isCollected = false;
    }

    protected virtual void OnDisable()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Rotating();
        Floating();
    }
    protected void Rotating()
    {
        if (isRotating)
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
        }
    }
    protected void Floating()
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
    protected IEnumerator TriggerCollectEffect(Vector3 targetPos)
    {
        isCollected = true;
        SwitchAnimation(false);
        Vector3 originalPostion = transform.position;
        Vector3 dir = targetPos - Vector3.ProjectOnPlane(transform.position, Vector3.up*0.3f);
        float elapsedTime = 0;

        while(elapsedTime <= flyAwayDurtion)
        {
            //fly away from player
            transform.position = originalPostion + elapsedTime/flyAwayDurtion * dir*flyDistance;
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
    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            SwitchAnimation(false);
            if (!isCollected)
            {
                StartCoroutine(TriggerCollectEffect(other.transform.position+Vector3.up*1.5f));
            }
            else
            {
                Actions.OnCollected?.Invoke(this);
                VFX.transform.position = transform.position;
                VFX.Play();
                collectionManager.collectionPools[(int)collectionType].Release(this);
            }
        }
    }
}
