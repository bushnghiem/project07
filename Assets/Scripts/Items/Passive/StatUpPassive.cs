using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatAdjustment
{
    public ShipStatType statType;

    [Header("Bonuses")]
    public float flatBonus;
    public float percentBonus;
}

[CreateAssetMenu(menuName = "Items/Passive/Stat Up")]
public class StatUpPassive : PassiveItem
{
    [Header("Stat Adjustments")]
    public List<StatAdjustment> statAdjustments = new();

    public override void ApplyEffect(Unit unit)
    {
        if (unit is not UnitBase unitBase)
            return;

        foreach (var adjustment in statAdjustments)
        {
            StatModifier modifier = new StatModifier
            {
                statType = adjustment.statType,
                flatBonus = adjustment.flatBonus,
                percentBonus = adjustment.percentBonus,
                sourceID = itemID
            };

            unitBase.AddStatModifier(modifier);
        }
    }

    public override void RemoveEffect(Unit unit)
    {
        if (unit is UnitBase unitBase)
        {
            unitBase.RemoveModifiersFromSource(itemID);
        }
    }
}