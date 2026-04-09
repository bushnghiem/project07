using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Shotgun")]
public class ShotgunEffect : Effect
{
    public int pelletCount = 6;
    public float spreadAngle = 20f;
    public Projectile pelletProjectile; // optional

    public override void Execute(EffectContext context)
    {
        Projectile projToUse = pelletProjectile != null
            ? pelletProjectile
            : context.owner.GetProjectile();

        for (int i = 0; i < pelletCount; i++)
        {
            Vector3 dir = GetSpreadDirection(context);

            float force = Random.Range(0.8f, 1.2f);

            ProjectileSpawnEvent.OnProjectileSpawn?.Invoke(
                context.position,
                dir,
                force,
                projToUse,
                context.owner
            );
        }
    }

    private Vector3 GetSpreadDirection(EffectContext context)
    {
        Vector3 forward = context.source.transform.forward;

        float angle = Random.Range(-spreadAngle, spreadAngle);

        return Quaternion.Euler(0f, angle, 0f) * forward;
    }
}