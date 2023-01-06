using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;


public class PictureSwitcher : MonoBehaviour
{
    public Transform levelPictures;
    public SceneController sceneController;

    public Button leftButton;
    public Button rightButton;
    public Button selectButton;

    public List<RawImage> levels = new List<RawImage>();
    public Stack<RawImage> leftStack = new Stack<RawImage>();
    public Stack<RawImage> rightStack = new Stack<RawImage>();

    public Vector2 centerPos;
    public Vector2 leftPos;
    public Vector2 rightPos;

    public RawImage centerPicture;
    public Animator centerPictureAnimator;

    private int levelCount = 0;
    private void Start()
    {
        sceneController = GameObject.FindObjectOfType<SceneController>();

        centerPos = new Vector2(0, 50);
        leftPos = new Vector2(-320, 0);
        rightPos = new Vector2(320, 0);

        //push all levels to rightStack
        foreach (RawImage level in levels)
        {
            centerPicture = GameObject.Instantiate(level, levelPictures);
            centerPicture.gameObject.name = level.gameObject.name + levelCount.ToString();
            levelCount++;
            centerPicture.rectTransform.anchoredPosition = rightPos;
            rightStack.Push(centerPicture);
            rightStack.Peek().gameObject.SetActive(false);
        }

        rightStack.Peek().gameObject.SetActive(true);
        PopLevel(rightStack);

        #region setup button
        leftButton.onClick.AddListener(()=> {
            if (rightStack.Count >0)
            {
                PushLevel(leftStack);
                PopLevel(rightStack);
            }

        });
        rightButton.onClick.AddListener(()=> {
            if (leftStack.Count >0)
            {
                PushLevel(rightStack);
                PopLevel(leftStack);
            }
        });
        selectButton.onClick.AddListener(()=> {
            sceneController.LoadLevel("Playground");
        });
        #endregion
        //init
    }

    public void PushLevel(Stack<RawImage> _stack)
    {
        string animationParameter;
        if (_stack.Equals(leftStack))
        {
            animationParameter = "SwitchLeft";
        }
        else
        {
            animationParameter = "SwitchRight";
        }
        centerPictureAnimator = centerPicture.gameObject.GetComponent<Animator>();
        centerPictureAnimator.SetTrigger(animationParameter);

        if (_stack.Count > 0)
        {
            _stack.Peek().gameObject.SetActive(false);
        }
        _stack.Push(centerPicture);

    }

    public void PopLevel(Stack<RawImage> _stack)
    {
        centerPicture = _stack.Pop();
        if (_stack.Count > 0)
        {
            _stack.Peek().gameObject.SetActive(true);
        }

        string animationParameter;
        if (_stack.Equals(leftStack))
        {
            animationParameter = "SwitchLeftBack";
        }
        else
        {
            animationParameter = "SwitchRightBack";
        }
        centerPictureAnimator = centerPicture.gameObject.GetComponent<Animator>();
        centerPictureAnimator.SetTrigger(animationParameter);
    }


}
