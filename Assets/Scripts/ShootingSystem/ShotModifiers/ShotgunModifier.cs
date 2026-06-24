using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Shot Modifiers/Shotgun")]
public class ShotgunModifier : ShotModifier
{
    public int pelletCount = 5;
    public float spreadAngle = 30f;

    public override void Modify(
        ShotPattern pattern,
        UnitBase shooter)
    {
        List<ProjectileSpawnData> result =
            new();

        foreach (var shot in pattern.projectiles)
        {
            for (int i = 0; i < pelletCount; i++)
            {
                float t =
                    pelletCount == 1
                    ? 0.5f
                    : i / (float)(pelletCount - 1);

                float angle =
                    Mathf.Lerp(
                        -spreadAngle * 0.5f,
                        spreadAngle * 0.5f,
                        t
                    );

                ProjectileSpawnData pellet =
                    shot;

                pellet.direction =
                    Quaternion.AngleAxis(
                        angle,
                        Vector3.up
                    ) * shot.direction;

                result.Add(pellet);
            }
        }

        pattern.projectiles = result;
    }
}