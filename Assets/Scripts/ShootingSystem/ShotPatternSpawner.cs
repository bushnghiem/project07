using UnityEngine;

public static class ShotPatternSpawner
{
    public static void Spawn(
        ShotPattern pattern,
        UnitBase owner)
    {
        foreach (var shot in pattern.projectiles)
        {
            Vector3 spawnPosition =
                owner.Position +
                shot.direction.normalized *
                (
                    owner.Template.CollisionRadius +
                    owner.Template.ProjectileSpawnRadius
                );

            spawnPosition += shot.spawnOffset;

            ProjectileSpawnEvent.OnProjectileSpawn?.Invoke(
                new ProjectileSpawnRequest
                {
                    Position = spawnPosition,
                    Direction = shot.direction,
                    Force = shot.force,
                    Projectile = shot.projectile,
                    Owner = owner,
                    ActionContext = pattern.actionContext
                }
            );
        }
    }
}
