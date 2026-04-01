using UnityEngine;

[CreateAssetMenu(menuName = "Items/Passive/Heal Every Turn")]
public class HealEveryTurn : PassiveItem
{
    [Header("Healing Settings")]
    public float healAmount = 5f;

    private UnitBase target;

    public override void ApplyEffect(Unit unit)
    {
        if (unit is UnitBase unitBase)
        {
            target = unitBase;

            EventBus.OnEvent += OnUnitEvent;
        }
    }

    private void OnUnitEvent(UnitEvent evt)
    {
        // Only act on TurnStart for this specific unit
        if (evt.source == target && evt.type == UnitEventType.TurnStart)
        {
            target.Heal(healAmount);
            Debug.Log($"{target.name} healed at start of turn for {healAmount} due to {itemName}");
        }
    }

    public override void RemoveEffect(Unit unit)
    {
        if (unit is UnitBase unitBase && target == unitBase)
        {
            EventBus.OnEvent -= OnUnitEvent;
            target = null;
        }
    }
}