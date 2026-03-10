using UnityEngine;

public abstract class PassiveItem : Item
{
    public override void OnAcquire(UnitBase unit)
    {
        unit.EquipPassive(this);
    }

    public abstract void ApplyEffect(Unit unit);

    public virtual void RemoveEffect(Unit unit) { }
}
