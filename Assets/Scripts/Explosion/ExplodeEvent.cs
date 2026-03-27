using System;
using UnityEngine;

public static class ExplodeEvent
{
    public static Action<ExplosionStats, Vector3> OnExplode;

    public static void Trigger(ExplosionStats stats, Vector3 position)
    {
        OnExplode?.Invoke(stats, position);
    }
}