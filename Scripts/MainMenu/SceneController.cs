using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum SceneList
{
    Playground,
    MainMenu,
}
public enum SceneTransitionType
{
    Cover,
    Fade,
}

public class SceneController : MonoBehaviour
{
    public Transform panelsHolderTransform;
    public static Transform backGroundBlocker;

    CanvasGroup canvasGroup;
    Image image;
    public GameObject backGroundPrefab;
    GameObject backGround;

    public float transitionTime = 1f;
    [SerializeField] float transitionTimeOffset = 0.1f;
    public float transitionSmoothness = 50;


    private void Start()
    {
        image = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (SceneManager.sceneCount == 0)
        {
            panelsHolderTransform = GameObject.Find("Panels").transform;
            backGroundBlocker = GameObject.Find("BackgroundBlocker").transform;
            backGroundBlocker.gameObject.SetActive(false);
        }
        StartCoroutine(SceneTransition(SceneTransitionType.Fade));
    }

    public void LoadLevel(SceneList scene)
    {
        Time.timeScale = 1;
        StartCoroutine(LevelTransition(scene));
    }
    IEnumerator LevelTransition(SceneList scene)
    {
        StartCoroutine(SceneTransition(SceneTransitionType.Cover));
        switch (scene)
        {
            case SceneList.Playground :
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case SceneList.MainMenu :
                backGround = Instantiate(backGroundPrefab, panelsHolderTransform);
                backGround.transform.SetAsFirstSibling();
                Cursor.lockState = CursorLockMode.None;
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(transitionTime+transitionTimeOffset);
        SceneManager.LoadScene(scene.ToString());
    }

    IEnumerator SceneTransition(SceneTransitionType type)
    {
        float startAlpha;
        float EndAlpha;
        switch (type)
        {
            case SceneTransitionType.Cover:
                startAlpha = 0;
                EndAlpha = 1;
                break;
            case SceneTransitionType.Fade:
            default:
                startAlpha = 1;
                EndAlpha = 0;
                break;
        }

        float duration = transitionTime / transitionSmoothness;
        WaitForSeconds wait = new WaitForSeconds(duration);
        for (int i = 0; i <= transitionSmoothness; i++)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, EndAlpha, 1 / transitionSmoothness * i);
            yield return wait;
        }
    }
}
