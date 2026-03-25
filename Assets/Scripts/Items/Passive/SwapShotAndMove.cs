using UnityEngine;

[CreateAssetMenu(menuName = "Items/Passive/SwapShot&Move")]
public class SwapShotAndMove : PassiveItem
{
    StatModifier addedShotStrength;
    StatModifier addedMoveStrength;
    StatModifier subtractedShotStrength;
    StatModifier subtractedMoveStrength;

    public override void ApplyEffect(Unit unit)
    {
        if (unit is UnitBase unitBase)
        {
            float shotStrength = unitBase.GetStat(ShipStatType.ShotStrength);
            float moveStrength = unitBase.GetStat(ShipStatType.MoveStrength);

            addedShotStrength = new StatModifier
            {
                statType = ShipStatType.ShotStrength,
                flatBonus = moveStrength,
                percentBonus = 0.0f,
                sourceID = itemID
            };

            addedMoveStrength = new StatModifier
            {
                statType = ShipStatType.MoveStrength,
                flatBonus = shotStrength,
                percentBonus = 0.0f,
                sourceID = itemID
            };

            subtractedShotStrength = new StatModifier
            {
                statType = ShipStatType.ShotStrength,
                flatBonus = -shotStrength,
                percentBonus = 0.0f,
                sourceID = itemID
            };

            subtractedMoveStrength = new StatModifier
            {
                statType = ShipStatType.MoveStrength,
                flatBonus = -moveStrength,
                percentBonus = 0.0f,
                sourceID = itemID
            };

            unitBase.AddStatModifier(addedMoveStrength);
            unitBase.AddStatModifier(addedShotStrength);
            unitBase.AddStatModifier(subtractedMoveStrength);
            unitBase.AddStatModifier(subtractedShotStrength);
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