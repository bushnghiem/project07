using UnityEngine;

[CreateAssetMenu(menuName = "Items/Passive/SwapShot&Move")]
public class SwapShotAndMove : PassiveItem
{
    private StatModifier addedShotStrength;
    private StatModifier addedMoveStrength;
    private StatModifier subtractedShotStrength;
    private StatModifier subtractedMoveStrength;

    public override void ApplyEffect(Unit unit)
    {
        if (unit is not UnitBase unitBase)
            return;

        float shotStrength =
            unitBase.GetStat(
                ShipStatType.ShotStrength);

        float moveStrength =
            unitBase.GetStat(
                ShipStatType.MoveStrength);

        addedShotStrength = new StatModifier(
            ShipStatType.ShotStrength,
            StatModifierOperation.Flat,
            moveStrength,
            itemID,
            itemName
        );

        addedMoveStrength = new StatModifier(
            ShipStatType.MoveStrength,
            StatModifierOperation.Flat,
            shotStrength,
            itemID,
            itemName
        );

        subtractedShotStrength = new StatModifier(
            ShipStatType.ShotStrength,
            StatModifierOperation.Flat,
            -shotStrength,
            itemID,
            itemName
        );

        subtractedMoveStrength = new StatModifier(
            ShipStatType.MoveStrength,
            StatModifierOperation.Flat,
            -moveStrength,
            itemID,
            itemName
        );

        unitBase.AddStatModifier(
            addedMoveStrength);

        unitBase.AddStatModifier(
            addedShotStrength);

        unitBase.AddStatModifier(
            subtractedMoveStrength);

        unitBase.AddStatModifier(
            subtractedShotStrength);
    }

    public override void RemoveEffect(Unit unit)
    {
        if (unit is UnitBase unitBase)
        {
            unitBase.RemoveModifiersFromSource(itemID);
        }
    }
}
