using UnityEngine;

public static class ShotPatternSpawner
{
    public static void Spawn(
        ShotPattern pattern,
        UnitBase owner)
    {
        foreach (var shot in pattern.projectiles)
        {
            ProjectileSpawnEvent.OnProjectileSpawn?.Invoke(
            new ProjectileSpawnRequest
            {
                Position = shot.position,
                Direction = shot.direction,
                Force = shot.force,
                Projectile = shot.projectile,
                Owner = owner,
                Attack = pattern.attackContext
            });
        }
    }
}
