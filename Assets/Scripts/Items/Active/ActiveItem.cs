using UnityEngine;

public abstract class ActiveItem : Item
{
    [Header("Targeting Visuals (Optional)")]
    public GameObject previewPrefab;
    public Color validColor = Color.green;
    public Color invalidColor = Color.red;

    [Header("Direction Targeting (Optional)")]
    public float coneAngle = 45f;   // degrees
    public float effectRadius = 8f;

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