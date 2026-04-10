using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Shotgun")]
public class ShotgunEffect : Effect
{
    [Header("Shotgun Settings")]
    public int projectileCount = 5;
    public float spreadAngle = 30f;

    [Header("Force")]
    public float forceMultiplier = 1f;
    public Vector2 forceVariance = new Vector2(0.9f, 1.1f);

    [Header("Spawn")]
    public float spawnRadius = 1f;

    public override void Execute(EffectContext context)
    {
        var unit = context.owner;
        if (unit == null) return;

        var projectile = unit.GetProjectile();
        if (projectile == null) return;

        Vector3 baseDir = context.direction.normalized;
        Vector3 origin = context.position;

        Vector3 right = Vector3.Cross(Vector3.up, baseDir).normalized;

        for (int i = 0; i < projectileCount; i++)
        {
            float angleOffset = Random.Range(-spreadAngle * 0.5f, spreadAngle * 0.5f);

            Vector3 dir = Quaternion.Euler(0f, angleOffset, 0f) * baseDir;

            Vector2 circle = Random.insideUnitCircle * 1.2f;
            Vector3 radialOffset =
                right * circle.x +
                Vector3.up * circle.y * 0.3f;

            float forwardJitter = Random.Range(0.2f, 1.2f);

            Vector3 spawnPos =
                origin +
                dir * (spawnRadius + forwardJitter) +
                radialOffset;

            float force = context.force *
                          forceMultiplier *
                          Random.Range(forceVariance.x, forceVariance.y);

            ProjectileSpawnEvent.OnProjectileSpawn?.Invoke(
                spawnPos,
                dir,
                force,
                projectile,
                context.owner
            );
        }
    }
}