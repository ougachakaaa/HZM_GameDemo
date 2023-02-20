using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    //manage game ui system: pause, gameover, player upgrade

    [SerializeField]Transform playerTransform;
    StarterAssetsInputs _input;
    PlayerLivingEntity playerLivingEntity;

    Transform canvas;
    [SerializeField] SceneController sceneController;

    //pause
    Transform pausePanel;
    Button resumeButton;
    Button quitButton;
    bool isQuit;

    //gameover
    CharacterStatus characterStatus;
    Timer timer;
    Transform gameoverPanel;
    Button restartButton;
    Button menuButton;
    TextMeshProUGUI gameoverTimeText;
    TextMeshProUGUI gameoverEnemyKilledText;

    //levelUp
    bool levelUpPause;
    public Transform levelUpPanel;
    [SerializeField] TextMeshProUGUI ATK;
    [SerializeField] TextMeshProUGUI DEF;
    [SerializeField] TextMeshProUGUI GunLevel;
    [SerializeField] TextMeshProUGUI AutoAimLevel;

    [SerializeField] Weapon _gun;
    [SerializeField] Weapon _autoAim;



    private void OnEnable()
    {
        Actions.OnPlayerDie += Gameover;
    }
    private void OnDisable()
    {
        Actions.OnPlayerDie -= Gameover;
    }


    // Start is called before the first frame update
    void Start()
    {
        _input = playerTransform.GetComponent<StarterAssetsInputs>();
        playerLivingEntity = playerTransform.GetComponent<PlayerLivingEntity>();
        //levelUpPanel = FindObjectOfType<LevelUpManager>();

        canvas = GameObject.Find("Canvas").transform;
        sceneController = Instantiate<SceneController>(sceneController,canvas);
        timer = canvas.GetComponentInChildren<Timer>();
        characterStatus = canvas.GetComponentInChildren<CharacterStatus>();

        pausePanel = canvas.Find("PausePanel");
        PauseInit(pausePanel);

        gameoverPanel = canvas.Find("GameoverPanel");
        GameoverInit(gameoverPanel);

        Actions.OnLevelUp += CallLevelUpPanel;

        UpgradePlayerProperty();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!Actions.IsPlayerDead)
        {
            PauseCheck();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    //pause stuff
    void PauseCheck()
    {

        if (_input.pause || levelUpPause)
        {
            Cursor.lockState = CursorLockMode.None;
            pausePanel.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            if (!isQuit)
            {
                Cursor.lockState = CursorLockMode.Locked;
                pausePanel.gameObject.SetActive(false);
            }
            Time.timeScale = 1;
        }
    }
    void PauseInit(Transform pausePanel)
    {
        isQuit = false;
        //initialize button
        resumeButton = pausePanel.Find("ResumeButton").GetComponent<Button>();
        quitButton = pausePanel.Find("QuitButton").GetComponent<Button>();

        resumeButton.onClick.AddListener(() => {
            _input.pause = false;
        });
        quitButton.onClick.AddListener(()=> {
            _input.pause = false;
            isQuit = true;
            sceneController.LoadLevel(SceneList.MainMenu);
        });
    }
    void GameoverInit(Transform gameoverPanel)
    {
        //initialize result display
        gameoverTimeText = gameoverPanel.Find("GameoverTimeText").GetComponent<TextMeshProUGUI>();
        gameoverEnemyKilledText = gameoverPanel.Find("GameoverEnemyKilledText").GetComponent<TextMeshProUGUI>();

        //initialize button
        restartButton = gameoverPanel.Find("RestartButton").GetComponent<Button>();
        menuButton = gameoverPanel.Find("MenuButton").GetComponent<Button>();

        restartButton.onClick.AddListener(() => {
            sceneController.LoadLevel(SceneList.Playground);
            Debug.Log("restart button here");
        });
        menuButton.onClick.AddListener(() => {
            Debug.Log("menu button here");
            sceneController.LoadLevel(SceneList.MainMenu);
        });

    }
    void Gameover()
    {
        timer.isTimerRunning = false;
        gameoverPanel.gameObject.SetActive(true);
        gameoverTimeText.text = timer.timerText.text;
        gameoverEnemyKilledText.text = characterStatus.enemyKilledCount.ToString();

    }


    public void CallLevelUpPanel()
    {
        if (levelUpPanel !=null)
        {
            levelUpPanel.gameObject.SetActive(true);
            levelUpPause = true;

        }
    }
    public void UpgradePlayerProperty()
    {

        levelUpPanel.gameObject.SetActive(false);
        levelUpPause = false;
        ATK.text = $"ATK:{playerLivingEntity.attackPoint}";
        DEF.text = $"DEF:{playerLivingEntity.defensePoint}";
        GunLevel.text = $"Gun:Lv.{_gun.currentLevel}";
        AutoAimLevel.text = $"Auto:Lv.{_autoAim.currentLevel}";
    }
}

