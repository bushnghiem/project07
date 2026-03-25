using UnityEngine;

[CreateAssetMenu(menuName = "Items/Passive/Stat Up")]
public class StatUpPassive : PassiveItem
{
    [Header("Stat Settings")]
    public ShipStatType statType;
    public float statChangeAmount;

    private StatModifier createdModifier;

    public override void ApplyEffect(Unit unit)
    {
        if (unit is UnitBase unitBase)
        {
            createdModifier = new StatModifier
            {
                statType = statType,
                flatBonus = statChangeAmount,
                percentBonus = 0f,
                sourceID = itemID
            };

            unitBase.AddStatModifier(createdModifier);
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