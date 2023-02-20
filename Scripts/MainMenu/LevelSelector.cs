using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    //level parameters
    public LevelParamHolder levelHolder;
    public GameMode mode;

    //buttons
    Button leftButton;
    Button rightButton;
    Button selectButton;

    public Button modeButton_0;
    public Button modeButton_1;
    public Transform startGamePanel;

    //levels
    bool hasGenerateLevel;
    public TextMeshProUGUI levelModeHeader;
    RectTransform levelPicHolderRT;
    RectTransform levelIndicator;
    public GameObject levelPrefab;
    public int levelCount;
    GameObject[] levels;
    float levelPicDistance;
    public int currentLevel =0;

    public float switchDuration=0.2f;
    bool isSwitchDone;
    Queue<int> switchCommands;

    //scene
    public SceneController sceneController;


    private void Start()
    {
        //get levelHolder
        levelHolder = FindObjectOfType<LevelParamHolder>();
        startGamePanel = GameObject.Find("StartGamePanel").transform;

        //get scenecontroller
        sceneController = GameObject.FindObjectOfType<SceneController>();
        

        //get buttons
        leftButton = transform.Find("LeftButton").GetComponent<Button>();
        rightButton = transform.Find("RightButton").GetComponent<Button>();
        selectButton = transform.Find("SelectButton").GetComponent<Button>();
        modeButton_0 = startGamePanel.Find("SurviveMode").GetComponent<Button>();
        modeButton_1 = startGamePanel.Find("DiamondCollect").GetComponent<Button>();


        //setup level pictures
        levelPicHolderRT = transform.Find("LevelPicHolder").GetComponent<RectTransform>();
        levelIndicator = transform.Find("LevelIndicator").GetComponent<RectTransform>();
        levelPicDistance = (levelPrefab.transform as RectTransform).rect.width+40;
        switchCommands = new Queue<int>();



        //setup button click event
        leftButton.onClick.AddListener(() =>
        {
            SwitchLevel(-1);
        });
        rightButton.onClick.AddListener(() =>
        {
            SwitchLevel(1);
        });

        //set select button event
        selectButton.onClick.AddListener(() =>
        {
            SetLevelHolder();
            sceneController.LoadLevel(SceneList.Playground);
        });
        

        //mode selecting event
        modeButton_0.onClick.AddListener(() =>
        {
            GenerateLevels();
            SetGameMode(0);
        });

        modeButton_1.onClick.AddListener(() =>
        {
            GenerateLevels();
            SetGameMode(1);
        });

        hasGenerateLevel = false;
        selectButton.gameObject.SetActive(false);
        levelIndicator.gameObject.SetActive(false);
    }
    void SetLevelHolder()
    {
        levelHolder.levelIndex = currentLevel;
        levelHolder.mode = mode;
    }

    public void SetGameMode(int i)
    {
        selectButton.gameObject.SetActive(true);
        mode = (GameMode)i;
        levelModeHeader.text = mode.ToString();
    }

    void GenerateLevels()
    {
        if (!hasGenerateLevel)
        {
            levels = new GameObject[levelCount];
            for (int i = 0; i < levelCount; i++)
            {

                levels[i] = Instantiate(levelPrefab, levelPicHolderRT);
                levels[i].name = $"LEVEL {i + 1}";
                levels[i].transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = $"LEVEL\n{i + 1}";
                (levels[i].transform as RectTransform).anchoredPosition = levelIndicator.anchoredPosition + 
                    new Vector2((i-currentLevel) * levelPicDistance,0);
            }
            isSwitchDone = true;
            hasGenerateLevel = true;
            levelIndicator.gameObject.SetActive(true);
        }
    }

    void SwitchLevel(int dir)
    {
        if (levels != null)
        {
            currentLevel += dir ;
            if (currentLevel < 0 || currentLevel >= levels.Length)
            {
                currentLevel -= dir;
                return;
            }
            else
            {
                switchCommands.Enqueue(dir);
            }

            if (isSwitchDone && switchCommands.Count > 0)
            {
                StartCoroutine(MoveLevelPics(switchCommands.Dequeue(), switchDuration));
            }

        }
    }

    IEnumerator MoveLevelPics(int dir,float duration = 0.25f)
    {
        float timer = 0f;
        isSwitchDone = false;
        RectTransform[] picRTs = new RectTransform[levelCount];
        Vector2[] originPositions = new Vector2[levelCount];
        Vector2[] targetPositions = new Vector2[levelCount];
        for (int i = 0; i < levelCount; i++)
        {
            picRTs[i] = levels[i].transform as RectTransform;
            originPositions[i] = picRTs[i].anchoredPosition;
            targetPositions[i] = originPositions[i] - dir * new Vector2(levelPicDistance, 0);
        }
        while (timer<duration)
        {
            timer += Time.deltaTime;
            float x = timer / duration;
            for (int i = 0; i < levels.Length; i++)
            {
                picRTs[i].anchoredPosition = Vector2.Lerp(originPositions[i], targetPositions[i], -Mathf.Pow(x-1,2)+1);
            }
            yield return null;
        }
        if (switchCommands.Count > 0)
        {
            StartCoroutine(MoveLevelPics(switchCommands.Dequeue(), switchDuration));
        }
        else
        {
            isSwitchDone = true;
        }
        yield return null;

    }

}

