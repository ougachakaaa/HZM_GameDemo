using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject sceneTransition;
    public GameObject cover;
    public Animator transitionAnimator;
    public Transform canvasTransform;

    public float transitionTime = 1f;

    private void Awake()
    {
        canvasTransform = gameObject.transform;
        cover = Instantiate(sceneTransition, canvasTransform);
        transitionAnimator = cover.GetComponent<Animator>();
    }

    IEnumerator LevelTransition(string sceneName)
    {

        transitionAnimator.Play("UnloadScene");
        Time.timeScale = 1;
        switch (sceneName)
        {
            case "Playground" :
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case "MainMenu" :
                Cursor.lockState = CursorLockMode.None;
                break;
            default:
                break;

        }
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LevelTransition(sceneName));
    }
}
