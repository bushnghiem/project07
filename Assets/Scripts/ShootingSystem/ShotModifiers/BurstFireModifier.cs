using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Shot Modifiers/Burst Fire")]
public class BurstFireModifier : ShotModifier
{
    [Min(1)]
    public int burstCount = 3;

    [Header("Burst Feel")]
    public float forwardOffsetStep = 0.15f;

    public float lateralOffset = 0.02f;

    public override void Modify(
        ShotPattern pattern,
        UnitBase shooter)
    {
        List<ProjectileSpawnData> result = new();

        foreach (var shot in pattern.projectiles)
        {
            Vector3 forward =
                shot.direction.normalized;

            Vector3 right =
                Vector3.Cross(
                    Vector3.up,
                    forward
                ).normalized;

            // Fallback for unusual/vertical directions.
            if (right.sqrMagnitude < 0.001f)
            {
                right =
                    Vector3.Cross(
                        Vector3.forward,
                        forward
                    ).normalized;
            }

            for (int i = 0; i < burstCount; i++)
            {
                float centeredIndex =
                    i - (burstCount - 1) * 0.5f;

                var burstShot = shot;

                burstShot.spawnOffset =
                    shot.spawnOffset
                    + forward * (forwardOffsetStep * i)
                    + right * (lateralOffset * centeredIndex);

                burstShot.force = shot.force;

                result.Add(burstShot);
            }
        }

        pattern.projectiles = result;
    }
}
