using UnityEngine;

public abstract class ActiveItem : Item
{
    public int cooldownTurns = 2;
    public float range = 10f;

    public abstract ItemTargetType TargetType { get; }
    private void OnEnable()
    {
        slotType = ItemSlotType.Active;
    }

    public override void OnAcquire(UnitBase unit)
    {
        unit.EquipActive(this);
    }

    public abstract void Execute(
        Unit user,
        ItemTargetData targetData
    );
}