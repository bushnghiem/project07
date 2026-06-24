using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Shot Modifiers/Burst Fire")]
public class BurstFireModifier : ShotModifier
{
    public int burstCount = 3;

    [Header("Burst Feel")]
    public float forwardOffsetStep = 0.15f;
    public float lateralJitter = 0.02f;
    public float forceVariance = 0.03f;

    public override void Modify(ShotPattern pattern, UnitBase shooter)
    {
        List<ProjectileSpawnData> result = new();

        foreach (var shot in pattern.projectiles)
        {
            Vector3 forward = shot.direction.normalized;

            Vector3 upReference = Vector3.up;

            if (Mathf.Abs(Vector3.Dot(forward, upReference)) > 0.95f)
                upReference = Vector3.forward;

            Vector3 right = Vector3.Cross(upReference, forward).normalized;
            Vector3 up = Vector3.Cross(forward, right).normalized;

            for (int i = 0; i < burstCount; i++)
            {
                float t = i;

                Vector3 posOffset =
                    shot.position
                    + forward * (forwardOffsetStep * t)
                    + right * Random.Range(-lateralJitter, lateralJitter);

                float forceJitter =
                    1f + Random.Range(-forceVariance, forceVariance);

                result.Add(new ProjectileSpawnData
                {
                    position = posOffset,
                    direction = forward,
                    force = shot.force * forceJitter,
                    projectile = shot.projectile
                });
            }
        }

        pattern.projectiles = result;
    }
}