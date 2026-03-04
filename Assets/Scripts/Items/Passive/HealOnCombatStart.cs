using UnityEngine;

[CreateAssetMenu(menuName = "Items/Passive/Heal On Combat Start")]
public class HealOnCombatStartPassive : PassiveItem
{
    [Header("Healing Settings")]
    public float healAmount = 5f;

    // ApplyEffect is called when the passive is equipped
    public override void ApplyEffect(Unit unit)
    {
        if (unit is UnitBase unitBase)
        {
            // Heal the unit immediately when combat starts
            unitBase.Heal(healAmount);
            Debug.Log($"{unitBase.name} healed {healAmount} from {itemName} at combat start");
        }
    }

    public override void RemoveEffect(Unit unit)
    {
        // No ongoing effect, nothing to remove
    }
}
