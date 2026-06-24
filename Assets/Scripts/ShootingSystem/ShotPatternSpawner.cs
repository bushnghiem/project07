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
                shot.position,
                shot.direction,
                shot.force,
                shot.projectile,
                owner
            );
        }
    }
}
