using UnityEngine;

public abstract class ActiveItem : Item
{
    public int cooldownTurns = 2;

    private void OnEnable()
    {
        slotType = ItemSlotType.Active;
    }

    public override void OnAcquire(UnitBase unit)
    {
        unit.EquipActive(this);
    }

    public virtual void Activate(Unit user, Unit target)
    {
    }
}