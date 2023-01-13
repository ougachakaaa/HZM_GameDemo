using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    None = 0,
    Dragon,
    Skeleton,
    Humanoid,
}

public class Monster : MonoBehaviour
{
    [SerializeField]
    private string _name;

    [SerializeField]
    private MonsterType _monsterTpye;
    [SerializeField]
    private int _age;
    [SerializeField]
    private float _health;
    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private bool _canEnterCombat;
    [SerializeField]
    private MonsterAbility[] _abilities;



    public string Name => _name;
    public MonsterType ThisType => _monsterTpye;
    public int Age => _age;
    public float Health => _health;
    public float Damage => _damage;
    public float Speed => _speed;
    public bool CanEnterCombat => _canEnterCombat;
    public MonsterAbility[] Abilities => _abilities;


}
