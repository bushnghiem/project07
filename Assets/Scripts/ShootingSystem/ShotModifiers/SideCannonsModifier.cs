using UnityEngine;

[CreateAssetMenu(menuName = "Shot Modifiers/Side Cannons")]
public class SideCannonsModifier : ShotModifier
{
    public float angle = 90f;

    public float lateralOffset = 1.5f;

    public override void Modify(ShotPattern pattern, UnitBase shooter)
    {
        int count = pattern.projectiles.Count;

        Vector3 right = shooter.transform.right;

        for (int i = 0; i < count; i++)
        {
            var shot = pattern.projectiles[i];

            Vector3 baseDir = shot.direction.normalized;

            Vector3 leftDir =
                (Quaternion.AngleAxis(-angle, Vector3.up) * baseDir).normalized;

            Vector3 rightDir =
                (Quaternion.AngleAxis(angle, Vector3.up) * baseDir).normalized;

            Vector3 leftPos =
                shot.position + right * lateralOffset;

            Vector3 rightPos =
                shot.position - right * lateralOffset;

            pattern.projectiles.Add(new ProjectileSpawnData
            {
                position = leftPos,
                direction = leftDir,
                force = shot.force,
                projectile = shot.projectile
            });

            pattern.projectiles.Add(new ProjectileSpawnData
            {
                position = rightPos,
                direction = rightDir,
                force = shot.force,
                projectile = shot.projectile
            });
        }
    }
}