using UnityEngine;

public static class ShotPatternFactory
{
    public static ShotPattern CreateBasicShot(
        UnitBase shooter,
        Projectile projectile,
        Vector3 direction,
        float force)
    {
        ShotPattern pattern = new();

        Vector3 spawnPos =
            shooter.Position +
            direction.normalized *
            (
                shooter.Template.CollisionRadius +
                shooter.Template.ProjectileSpawnRadius
            );

        pattern.projectiles.Add(
            new ProjectileSpawnData
            {
                position = spawnPos,
                direction = direction.normalized,
                force = force,
                projectile = projectile
            }
        );

        return pattern;
    }
}