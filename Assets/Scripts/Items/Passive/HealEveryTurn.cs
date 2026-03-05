using UnityEngine;

[CreateAssetMenu(menuName = "Items/Passive/Heal Every Turn")]
public class HealEveryTurn : PassiveItem
{
    [Header("Healing Settings")]
    public float healAmount = 5f;

    // ApplyEffect is called when the passive is equipped
    public override void ApplyEffect(Unit unit)
    {
        if (unit is UnitBase unitBase)
        {
            unitBase.OnStartOfTurn += Healing;
        }
    }

    public void Healing(UnitBase unitBase)
    {
        // Heal unit at start of their turn
        unitBase.Heal(healAmount);
        Debug.Log($"{unitBase.name} healed at start of turn for {healAmount} due to {itemName}");
    }

    public override void RemoveEffect(Unit unit)
    {
        if (unit is UnitBase unitBase)
        {
            unitBase.OnStartOfTurn -= Healing;
        }
    }
}
