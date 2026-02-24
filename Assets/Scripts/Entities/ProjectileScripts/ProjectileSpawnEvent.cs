using System;
using UnityEngine;

public static class ProjectileSpawnEvent
{
    public static Action<Vector3, Vector3, float, Projectile> OnProjectileSpawn; // Spawn Position, Direction, Force Strength, and stats
    public static Action<Entity> AddCamFollow; // Entity for camera to follow
}
