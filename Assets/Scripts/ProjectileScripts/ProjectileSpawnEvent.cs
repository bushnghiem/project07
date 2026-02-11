using System;
using UnityEngine;

public static class ProjectileSpawnEvent
{
    public static Action<Vector3, Vector3, float> OnProjectileSpawn;
}
