using UnityEngine;
using System.Text;

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
    public int chargeCost = 1;
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

    public override string GetTooltipText(int shopPrice)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(description);
        sb.AppendLine();
        sb.AppendLine($"Type: {slotType}");
        sb.AppendLine($"Price: {shopPrice}");
        sb.AppendLine($"Cooldown: {cooldownTurns} turns");
        sb.AppendLine($"Charge Cost: {chargeCost} turns");
        sb.AppendLine($"Target Type: {TargetType}°");

        switch (TargetType)
        {
            case ItemTargetType.Self:
                break;

            case ItemTargetType.Unit:
                sb.AppendLine($"Range: {range}");
                break;

            case ItemTargetType.Position:
                sb.AppendLine($"Range: {range}");
                break;

            case ItemTargetType.Direction:
                sb.AppendLine($"Cone Angle: {coneAngle}°");
                sb.AppendLine($"Radius: {effectRadius}");
                break;
        }

        return sb.ToString();
    }
}