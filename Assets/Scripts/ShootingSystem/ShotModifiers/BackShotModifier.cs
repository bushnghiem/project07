using UnityEngine;

[CreateAssetMenu(menuName = "Shot Modifiers/Back Shot")]
public class BackShotModifier : ShotModifier
{
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

            shot.direction *= -1f;

            pattern.projectiles.Add(shot);
        }
    }
}