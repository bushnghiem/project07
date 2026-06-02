using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Projectile Spawn")]
public class CleanProjectileSpawnEffect : Effect
{
    public float spreadAngle = 30f;
    public int projectileCount = 5;

    public override void Execute(EffectContext context)
    {
        Projectile projectile = context.owner.GetProjectile();

        if (projectile == null)
            return;

        for (int i = 0; i < projectileCount; i++)
        {
            float t =
                projectileCount == 1
                ? 0.5f
                : i / (float)(projectileCount - 1);

            float angle =
                Mathf.Lerp(
                    -spreadAngle * 0.5f,
                    spreadAngle * 0.5f,
                    t);

            Vector3 dir =
                Quaternion.AngleAxis(angle, Vector3.up)
                * context.direction.normalized;

            Vector3 spawnPos =
                context.position +
                dir.normalized * 2f;

            ProjectileSpawnEvent.OnProjectileSpawn?.Invoke(
                spawnPos,
                dir,
                context.force,
                context.owner.GetProjectile(),
                context.owner
            );
        }
    }
}
