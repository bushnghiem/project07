using UnityEngine;

[CreateAssetMenu(menuName = "Items/Passive/Heal On Kill")]
public class HealOnKill : PassiveItem
{
    [Header("Healing Settings")]
    public float healAmount = 50f;
    public UnitBase target;

    // ApplyEffect is called when the passive is equipped
    public override void ApplyEffect(Unit unit)
    {
        if (unit is UnitBase unitBase)
        {
            unitBase.OnStartOfTurn += ActivateHealingOnKill; // At start of user turn, activate heal on kill
            TurnEvent.OnNextTurn += DeactivateHealingOnKill; // At end of turn (called by battle manager, not unit), stop healing
            // Not units end of turn event because unit calls end of turn as soon as shot occurs which means any kills after will not be seen
        }
    }

    public void ActivateHealingOnKill(UnitBase unitBase)
    {
        target = unitBase; // set healing target to user
        DeathEvent.OnEntityDeath += Healing; // Connect entity death to healing
    }

    public void DeactivateHealingOnKill(Unit unit)
    {
        if (unit is UnitBase unitBase)
        {
            if (unitBase == target)
            {
                target = null;
                DeathEvent.OnEntityDeath -= Healing; // If unit turn that ended is the user, disconnect healing on entity death
            }
        }
    }

    public void Healing(Entity entity)
    {
        if (entity is UnitBase unitBase)
        {
            target.Heal(healAmount); // If entity was a unit, heal user
        }
    }

    public override void RemoveEffect(Unit unit)
    {
        if (unit is UnitBase unitBase)
        {
            target = null;
            unitBase.OnStartOfTurn -= ActivateHealingOnKill;
            TurnEvent.OnUnitTurnEnd -= DeactivateHealingOnKill;
        }
    }
}
