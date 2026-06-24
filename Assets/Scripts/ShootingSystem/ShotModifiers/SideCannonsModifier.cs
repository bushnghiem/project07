using UnityEngine;

[CreateAssetMenu(menuName = "Shot Modifiers/Side Cannons")]
public class SideCannonsModifier : ShotModifier
{
    public float angle = 90f;

    public override void Modify(
        ShotPattern pattern,
        UnitBase shooter)
    {
        int count =
            pattern.projectiles.Count;

        for (int i = 0; i < count; i++)
        {
            var shot =
                pattern.projectiles[i];

            var left = shot;
            var right = shot;

            left.direction =
                Quaternion.AngleAxis(
                    -angle,
                    Vector3.up
                ) * shot.direction;

            right.direction =
                Quaternion.AngleAxis(
                    angle,
                    Vector3.up
                ) * shot.direction;

            pattern.projectiles.Add(left);
            pattern.projectiles.Add(right);
        }
    }
}