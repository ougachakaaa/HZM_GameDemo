using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestManager : MonoBehaviour
{
    public static TestManager Instance;
    int num = 0;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance == null)
        {
            Instance = this;
            Debug.Log($"Manager ID : {Instance.GetInstanceID()},Manager Num : {Instance.num++}");
        }
        else
        {
            Debug.Log(Instance);
            Destroy(this.gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadScene("New Scene");
        }
    }
}
