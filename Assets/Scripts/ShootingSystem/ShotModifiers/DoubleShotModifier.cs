using UnityEngine;

[CreateAssetMenu(menuName = "Shot Modifiers/Double Shot")]
public class DoubleShotModifier : ShotModifier
{
    public override void Modify(
        ShotPattern pattern,
        UnitBase shooter)
    {
        int count =
            pattern.projectiles.Count;

        for (int i = 0; i < count; i++)
        {
            pattern.projectiles.Add(
                pattern.projectiles[i]
            );
        }
    }
}