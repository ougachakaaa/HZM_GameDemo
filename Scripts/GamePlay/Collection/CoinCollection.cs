using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class CoinCollection : Collection
{
    public int coinID;
    public int coinValue;


    protected override void OnEnable()
    {
        base.OnEnable();
        SetCoinParam();

    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    protected override void Update()
    {
        base.Update();
    }

    public void SetCoinParam()
    {
        coinValue = coinID + 1;
    }
}
