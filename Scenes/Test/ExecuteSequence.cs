using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class ExecuteSequence : MonoBehaviour
{
    string objName;
    static int thisNum=0;
    //public static Action<int> OnExecuteTest;
    private void OnEnable()
    {
        objName = name;
        Debug.Log($"Awake! Name:{objName}, ID:{GetInstanceID()}, Num:{thisNum}" );
        thisNum++;
        //DontDestroyOnLoad(this);
    }
/*    void Start()
    {
        Debug.Log("Start " + objName + ",  ID: " + GetInstanceID());
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }

}
