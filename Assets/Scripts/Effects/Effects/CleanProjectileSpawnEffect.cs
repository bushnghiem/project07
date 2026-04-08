using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Projectile Spawn")]
public class CleanProjectileSpawnEffect : Effect
{
    public Projectile projectile;
    public int count = 1;
    public float spread = 1f;

    public override void Execute(EffectContext context)
    {
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = angleStep * i;

            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            Vector3 spawnPos = context.position + dir * spread;

            ProjectileSpawnEvent.OnProjectileSpawn?.Invoke(
                spawnPos,
                dir,
                5f,
                projectile,
                context.owner
            );
        }
    }
}
