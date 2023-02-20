using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HpBar : MonoBehaviour
{
    PlayerLivingEntity _playerLivingEntity;
    TextMeshProUGUI hpText;
    Image hpFill;

    // Start is called before the first frame update
    void Start()
    {
        hpFill = transform.Find("HpFill").GetComponent<Image>();
        hpText = transform.Find("HpText").GetComponent<TextMeshProUGUI>();
        _playerLivingEntity = FindObjectOfType<PlayerLivingEntity>();
    }

    // Update is called once per frame
    public void UpdateHpBar()
    {
        //update hpfill
        hpFill.fillAmount = (float)_playerLivingEntity.CurrentHp / _playerLivingEntity.maxHp;
        hpText.text = _playerLivingEntity.CurrentHp.ToString() + "/" + _playerLivingEntity.maxHp.ToString();
    }


}
