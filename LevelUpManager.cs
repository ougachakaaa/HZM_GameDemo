using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class LevelUpManager : MonoBehaviour
{
    public Button buttonPrefab;
    public Button[] upgradeButtons;
    public int buttonCount;

    PlayerLivingEntity _playerLivingEntity;
    WeaponAutoAim _weaponAutoAim;
    WeaponGun _weaponGun;
    GameUIManager _gameUIManager;

    Action[] upgradeActHolders;
    Action[] upgrades;
    Dictionary<Action,string> upgradeDict;
    int[] indexes;
    // Start is called before the first frame update
    void Awake()
    {
        _playerLivingEntity = FindObjectOfType<PlayerLivingEntity>();
        _weaponAutoAim = FindObjectOfType<WeaponAutoAim>();
        _weaponGun = FindObjectOfType<WeaponGun>();
        _gameUIManager = FindObjectOfType<GameUIManager>();

        upgradeDict = new Dictionary<Action, string>();

        SubscribeUpgrade();

        upgradeActHolders = new Action[3];
        upgradeButtons = new Button[3];
        indexes = new int[upgradeDict.Count];
        for (int i = 0; i < indexes.Length; i++)
            indexes[i] = i;

        for (int i = 0; i < 3; i++)
        {
            upgradeButtons[i] = Instantiate(buttonPrefab, transform);
            (upgradeButtons[i].transform as RectTransform).anchoredPosition = (transform as RectTransform).anchoredPosition;
            int n = i;
            upgradeButtons[i].onClick.AddListener(() =>
            {
                Debug.Log(n);
                upgradeActHolders[n].Invoke();
                _gameUIManager.UpgradePlayerProperty();
            });
        }
    }

    private void OnEnable()
    {
        GenerateUpgradeButton(3);
    }

    // Update is called once per frame
    void GenerateUpgradeButton(int buttonCount)
    {
        buttonCount = Mathf.Clamp(buttonCount, 2, 3);
        //get 3 random number as indexes of upgrades
        indexes = Utility.ShuffleArray(indexes,Mathf.FloorToInt(Time.time));
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = indexes[i];
            //put random upgrade action to the action holder of the first button
            upgradeActHolders[i] = upgrades[randomIndex];
            TextMeshProUGUI upgradeText = upgradeButtons[i].GetComponentInChildren<TextMeshProUGUI>();

            //make the text of the button correspond to the Action
            upgradeText.text = upgradeDict[upgrades[randomIndex]];
        }

        //put button on right position in different cases
        switch (buttonCount)
        {
            case 3:
                upgradeButtons[0].gameObject.SetActive(true);
                upgradeButtons[1].gameObject.SetActive(true);
                upgradeButtons[2].gameObject.SetActive(true);

                (upgradeButtons[0].transform as RectTransform).localPosition = new Vector3(-400, 0,0);
                (upgradeButtons[1].transform as RectTransform).localPosition = new Vector3(0, 0, 0);
                (upgradeButtons[2].transform as RectTransform).localPosition = new Vector3(400, 0, 0);

                break;
            case 2:
                upgradeButtons[0].gameObject.SetActive(true);
                upgradeButtons[1].gameObject.SetActive(true);
                upgradeButtons[2].gameObject.SetActive(false);

                (upgradeButtons[0].transform as RectTransform).localPosition = new Vector3(-200, 0, 0);
                (upgradeButtons[1].transform as RectTransform).localPosition = new Vector3(200, 0, 0);

                break;
            default:
                break;
        }
    }

    void SubscribeUpgrade()
    {

        upgradeDict.Add(_playerLivingEntity.UpgradeATK,"ATK\nUPGRADE");
        upgradeDict.Add(_playerLivingEntity.UpgradeDEF, "DEF\nUPGRADE");
        upgradeDict.Add(_weaponAutoAim.UpgradeWeapon, "AutoAim\nUPGRADE");
        upgradeDict.Add(_weaponGun.UpgradeWeapon, "Gun\nUPGRADE");

        upgrades = new Action[upgradeDict.Count];
        upgradeDict.Keys.CopyTo(upgrades,0);
    }
}
