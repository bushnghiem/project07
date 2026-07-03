using UnityEngine;
using System.Text;

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

    // Optional instance-aware versions
    public virtual void ApplyEffect(Unit unit, PassiveItemInstance instance)
    {
        ApplyEffect(unit);
    }

    public virtual void RemoveEffect(Unit unit, PassiveItemInstance instance)
    {
        RemoveEffect(unit);
    }
}