using UnityEngine;

public static class ShotPatternFactory
{
    public static ShotPattern CreateBasicShot(
        Projectile projectile,
        Vector3 direction,
        float force)
    {
        ShotPattern pattern = new();

        pattern.projectiles.Add(
            new ProjectileSpawnData
            {
                direction = direction.normalized,
                force = force,
                spawnOffset = Vector3.zero,
                projectile = projectile
            }
        );

        return pattern;
    }
}
