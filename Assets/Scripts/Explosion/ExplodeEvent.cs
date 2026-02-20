using System;
using UnityEngine;

public static class ExplodeEvent
{
    public static Action<ExplosionStats, Vector3> OnExplode; // Explosion stats, Explosion position
}
