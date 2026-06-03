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
            unit.AddStatModifier(new StatModifier
            {
                statType = adjustment.statType,
                flatBonus = adjustment.flatBonus,
                percentBonus = adjustment.percentBonus,
                sourceID = instance.itemData.itemID
            });
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