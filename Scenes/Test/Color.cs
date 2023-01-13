using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject objA;
    public MeshRenderer renderA;
    public Color colorA;
    public Color targetColor;
    public int step = 0;
    public float alpha = 0.3f;

    private void Start()
    {
        renderA = objA.GetComponent<MeshRenderer>();
        colorA = renderA.material.color;
        targetColor = new Color(colorA.r, colorA.g, colorA.b, alpha);
        step = 0;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ++step;
            renderA.material.color = Color.Lerp(colorA,targetColor,step*0.1f);
            objA.transform.Translate(Vector3.up);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            objA.SetActive(!objA.activeSelf);
            Debug.Log(objA.activeSelf);
        }
    }


}
