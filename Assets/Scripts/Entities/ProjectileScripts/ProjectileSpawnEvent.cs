using System;
using UnityEngine;

public struct ProjectileSpawnRequest
{
    public Vector3 Position;
    public Vector3 Direction;
    public float Force;

    public Projectile Projectile;
    public UnitBase Owner;

    public AttackContext Attack;
}

public static class ProjectileSpawnEvent
{
    public static Action<ProjectileSpawnRequest> OnProjectileSpawn;
}
