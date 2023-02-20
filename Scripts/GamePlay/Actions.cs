using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Actions
{

    //enemy
    public static Func<Coord, Vector3> OnEnemySpawn;
    public static Action<EnemyController> OnEnemyDie;
    public static Action<float> OnEnemyAttack;
    public static Action<float> OnEnemySpawnRateIncrease;

    //player
    public static Func<PlayerState,PlayerState> OnPlayerAttacked;
    public static Action OnPlayerDie;
    public static Action OnLevelUp;
    public static Action<Projectile> OnProjectileHit;
    public static bool IsPlayerDead;

    //collection
    public static Action<Collection> OnCollected;

    //hit effect
    public static Action<Vector3> OnKnockBack;

}
