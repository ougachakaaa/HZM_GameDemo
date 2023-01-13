using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum ElementType
{
    None,
    Earth,
    Fire,
    Wind,
    Water,
    Heart,
}
[Serializable]
public class MonsterAbility
{
    [SerializeField]
    private string _name = "...";
    [SerializeField]
    private int _damage = 1;
    [SerializeField]
    private ElementType _elementType = ElementType.None;
}
