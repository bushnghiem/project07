using UnityEngine;

public abstract class PassiveItem : Item
{
    private void OnEnable()
    {
        slotType = ItemSlotType.Passive;
    }

    public override void OnAcquire(UnitBase unit)
    {
        unit.EquipPassive(this);
    }

    public abstract void ApplyEffect(Unit unit);

    public virtual void RemoveEffect(Unit unit) { }
}
