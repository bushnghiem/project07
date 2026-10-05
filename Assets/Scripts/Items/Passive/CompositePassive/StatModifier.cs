using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Modifiers/Stats")]
public class StatModifierModule : PassiveModifier
{
    public List<StatAdjustment> statAdjustments = new();

    public override void Apply(
    UnitBase unit,
    PassiveItemInstance instance)
    {
        foreach (var adjustment in statAdjustments)
        {
            unit.AddStatModifier(
                new StatModifier(
                    adjustment.statType,
                    adjustment.operation,
                    adjustment.value,
                    instance.itemData.itemID,
                    instance.itemData.itemName
                )
            );
        }
    }


    public override void Remove(
        UnitBase unit,
        PassiveItemInstance instance)
    {
        unit.RemoveModifiersFromSource(
            instance.itemData.itemID);
    }
}
