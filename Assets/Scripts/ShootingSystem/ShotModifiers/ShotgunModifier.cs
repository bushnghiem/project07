using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Shot Modifiers/Shotgun")]
public class ShotgunModifier : ShotModifier
{
    [Min(1)]
    public int pelletCount = 2;

    [Range(0f, 180f)]
    public float spreadAngle = 30f;

    public override void Modify(
        ShotPattern pattern,
        UnitBase shooter)
    {
        List<ProjectileSpawnData> result = new();

        foreach (var shot in pattern.projectiles)
        {
            Vector3 baseDirection =
                shot.direction.normalized;

            for (int i = 0; i < pelletCount; i++)
            {
                float t = pelletCount == 1
                    ? 0.5f
                    : i / (float)(pelletCount - 1);

                float angle =
                    Mathf.Lerp(
                        -spreadAngle * 0.5f,
                        spreadAngle * 0.5f,
                        t
                    );

                Vector3 direction =
                    Quaternion.AngleAxis(
                        angle,
                        Vector3.up
                    ) * baseDirection;

                var pellet = shot;

                pellet.direction =
                    direction.normalized;

                result.Add(pellet);
            }
        }

        pattern.projectiles = result;
    }
}
