using UnityEngine;

[CreateAssetMenu(menuName = "Items/Passive/Heal On Kill")]
public class HealOnKill : PassiveItem
{
    public float healAmount = 50f;

    private UnitBase owner;
    private bool active = false;

    public override void ApplyEffect(Unit unit)
    {
        owner = unit as UnitBase;
        if (owner == null) return;

        EventBus.OnEvent += HandleEvent;
    }

    public override void RemoveEffect(Unit unit)
    {
        EventBus.OnEvent -= HandleEvent;
        owner = null;
        active = false;
    }

    private void HandleEvent(UnitEvent e)
    {
        if (owner == null) return;

        switch (e.type)
        {
            case UnitEventType.TurnStart:
                if (e.source == owner)
                    active = true;
                break;

            case UnitEventType.TurnEnd:
                if (e.source == owner)
                    active = false;
                break;

            case UnitEventType.Death:
                if (!active) return;

                if (e.source != owner)
                {
                    owner.Heal(healAmount);
                    Debug.Log($"{owner.name} healed for {healAmount} due to {itemName}");
                }
                break;
        }
    }
}