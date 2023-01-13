using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HpBar : MonoBehaviour
{
    LivingEntity livingEntity;
    TextMeshProUGUI hpText;
    Image hpFill;

    // Start is called before the first frame update
    void Start()
    {
        hpFill = transform.Find("HpFill").GetComponent<Image>();
        hpText = transform.Find("HpText").GetComponent<TextMeshProUGUI>();
        livingEntity = FindObjectOfType<PlayerLivingEntity>();

    }

    // Update is called once per frame
    public void UpdateHpBar()
    {
        //update hpfill
        hpFill.fillAmount = livingEntity.CurrentHp / livingEntity.maxHp;
        hpText.text = livingEntity.CurrentHp.ToString() + "//" + livingEntity.maxHp.ToString();
    }


}
