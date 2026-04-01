using System;
using UnityEngine;

public static class ProjectileSpawnEvent
{
    public static Action<Vector3, Vector3, float, Projectile, UnitBase> OnProjectileSpawn;
    public static Action<Entity> AddCamFollow;
}
