using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Actions
{
    public static bool IsPlayerDead;

    public static Action<EnemyController> OnEnemyDie;
    public static Action<float> OnEnemyAttack;

    public static Func<PlayerState,PlayerState> OnPlayerAttacked;
    public static Action OnPlayerDie;

    //collection
    public static Action<Loot> OnLootCollected;

    //hit effect
    public static Action<Vector3> OnKnockBack;


}
