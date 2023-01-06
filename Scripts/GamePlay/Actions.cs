using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Actions
{
    public static Action OnEnemyDie;
    public static Func<PlayerState,PlayerState> OnPlayerAttacked;
    public static Action<Vector3> OnKnockBack;
}
