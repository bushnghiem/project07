using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Items/Passive/Composite Passive")]
public class CompositePassive : PassiveItem
{
    public List<PassiveModifier> modifiers = new();

    public override void ApplyEffect(Unit unit)
    {
        // Not used
    }

    public override void ApplyEffect(
        Unit unit,
        PassiveItemInstance instance)
    {
        if (unit is not UnitBase unitBase)
            return;

        foreach (var modifier in modifiers)
        {
            if (modifier == null)
                continue;

            modifier.Apply(unitBase, instance);
        }
    }

    public override void RemoveEffect(
        Unit unit,
        PassiveItemInstance instance)
    {
        if (unit is not UnitBase unitBase)
            return;

        foreach (var modifier in modifiers)
        {
            if (modifier == null)
                continue;

            modifier.Remove(unitBase, instance);
        }
    }
}