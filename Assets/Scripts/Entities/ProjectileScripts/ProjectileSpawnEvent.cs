using System;
using UnityEngine;

public static class ProjectileSpawnEvent
{
    public static Action<Vector3, Vector3, float> OnProjectileSpawn; // Spawn Position, Direction, and Force Strength
    public static Action<Entity> AddCamFollow; // Entity for camera to follow
}
