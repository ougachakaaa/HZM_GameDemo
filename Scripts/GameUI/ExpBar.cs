using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpBar : MonoBehaviour
{
    PlayerLivingEntity playerLivingEntity;
    TextMeshProUGUI levelText;
    Image expFill;



    // Start is called before the first frame update
    void Start()
    {
        expFill = transform.Find("ExpFill").GetComponent<Image>();
        levelText = transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
        playerLivingEntity = FindObjectOfType<PlayerLivingEntity>();
        UpdateExpBar();
    }

    // Update is called once per frame
    public void UpdateExpBar()
    {
        //update hpfill
        expFill.fillAmount = (float)playerLivingEntity.currentExpPoint / playerLivingEntity.expToLevelUp;
        levelText.text = $"Lv.{playerLivingEntity.currentLevel}";
    }




}
