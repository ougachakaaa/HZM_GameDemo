using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondCollection : Collection
{
    public int diamondID;
    public int diamondValue;



    protected override void OnEnable()
    {
        base.OnEnable();
        SetDiamondParam();

    }
    protected override void OnDisable()
    {
        base.OnDisable();

    }
    protected override void Update()
    {
        base.Update();
    }

    public void SetDiamondParam()
    {
        diamondValue=1;
    }
}
