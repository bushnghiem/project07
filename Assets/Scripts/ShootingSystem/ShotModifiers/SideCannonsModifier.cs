using UnityEngine;

[CreateAssetMenu(menuName = "Shot Modifiers/Side Cannons")]
public class SideCannonsModifier : ShotModifier
{
    [Range(0f, 180f)]
    public float angle = 90f;

    public float lateralOffset = 1.5f;

    public override void Modify(
        ShotPattern pattern,
        UnitBase shooter)
    {
        int count = pattern.projectiles.Count;

        for (int i = 0; i < count; i++)
        {
            var shot = pattern.projectiles[i];

            Vector3 baseDirection =
                shot.direction.normalized;

            Vector3 right =
                Vector3.Cross(
                    Vector3.up,
                    baseDirection
                ).normalized;

            // Fallback for unusual/vertical directions.
            if (right.sqrMagnitude < 0.001f)
            {
                right =
                    Vector3.Cross(
                        Vector3.forward,
                        baseDirection
                    ).normalized;
            }

            Vector3 leftDirection =
                (
                    Quaternion.AngleAxis(
                        -angle,
                        Vector3.up
                    ) * baseDirection
                ).normalized;

            Vector3 rightDirection =
                (
                    Quaternion.AngleAxis(
                        angle,
                        Vector3.up
                    ) * baseDirection
                ).normalized;

            var leftShot = shot;

            leftShot.direction = leftDirection;

            leftShot.spawnOffset =
                shot.spawnOffset -
                right * lateralOffset;

            var rightShot = shot;

            rightShot.direction = rightDirection;

            rightShot.spawnOffset =
                shot.spawnOffset +
                right * lateralOffset;

            pattern.projectiles.Add(leftShot);
            pattern.projectiles.Add(rightShot);
        }
    }
}
