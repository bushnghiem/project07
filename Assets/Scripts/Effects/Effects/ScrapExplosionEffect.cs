using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Effect/Scrap Explosion")]
public class ScrapProjectileSpawnEffect : Effect
{
    public Projectile projectile;

    public int count = 5;

    [Header("Spawn Radius")]
    public float minRadius = 0.5f;
    public float maxRadius = 2f;

    [Header("Separation")]
    public float minSeparation = 0.5f;

    [Header("Force")]
    public float minForce = 3f;
    public float maxForce = 10f;

    [Header("Direction")]
    public float randomAngle = 180f;
    public bool useDirectionalBias = true;

    [Header("Verticality")]
    public float maxUpward = 0.3f;

    [Header("Special Cases")]
    [Range(0f, 1f)] public float deadPieceChance = 0.2f;
    [Range(0f, 1f)] public float fastShardChance = 0.1f;
    public float deadForceMultiplier = 0.2f;
    public float fastForceMultiplier = 2.5f;

    public override void Execute(EffectContext context)
    {
        List<Vector3> usedPositions = new List<Vector3>();

        Vector3 baseDir = Vector3.forward;
        if (useDirectionalBias && context.source != null)
            baseDir = context.source.transform.forward;

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = context.position;
            for (int attempt = 0; attempt < 10; attempt++)
            {
                float angleOffset = Random.Range(-randomAngle, randomAngle);
                Vector3 dir = Quaternion.Euler(0, angleOffset, 0) * baseDir;

                dir.y = Random.Range(0f, maxUpward);
                dir.Normalize();

                float radius = Random.Range(minRadius, maxRadius);
                Vector3 candidate = context.position + dir * radius;

                bool tooClose = false;

                foreach (var pos in usedPositions)
                {
                    if (Vector3.Distance(pos, candidate) < minSeparation)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    spawnPos = candidate;
                    usedPositions.Add(candidate);

                    float force = Random.Range(minForce, maxForce);

                    if (Random.value < deadPieceChance)
                        force *= deadForceMultiplier;

                    if (Random.value < fastShardChance)
                        force *= fastForceMultiplier;

                    ProjectileSpawnEvent.OnProjectileSpawn?.Invoke(
                        spawnPos,
                        dir,
                        force,
                        projectile,
                        context.owner
                    );

                    break;
                }
            }
        }
    }
}