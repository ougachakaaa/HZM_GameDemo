using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelParamHolder : MonoBehaviour
{
    public static LevelParamHolder Instance;

    public GameMode mode;
    public int levelIndex;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
    public void ResetParamToMainMenu()
    {
        mode = GameMode.Demo;
        levelIndex = 0;
    }


    // Update is called once per frame

}
