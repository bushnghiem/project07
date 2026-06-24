using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Modifiers/Shot Modifiers")]
public class ShotModifierPassive : PassiveModifier
{
    public List<ShotModifier> modifiers =
        new();

    public override void Apply(
        UnitBase unit,
        PassiveItemInstance instance)
    {
        foreach (var modifier in modifiers)
        {
            ShotModifier runtime = Instantiate(modifier);

            unit.AddShotModifier(runtime);

            instance.grantedObjects.Add(runtime);
        }
    }

    public override void Remove(
        UnitBase unit,
        PassiveItemInstance instance)
    {
        foreach (var obj in instance.grantedObjects)
        {
            if (obj is ShotModifier modifier)
            {
                unit.RemoveShotModifier(modifier);

                Destroy(modifier);
            }
        }
    }
}