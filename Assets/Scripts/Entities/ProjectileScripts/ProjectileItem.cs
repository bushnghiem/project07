using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Projectile Item")]
public class ProjectileItem : Item
{
    public Projectile projectile;

    private void OnEnable()
    {
        slotType = ItemSlotType.Projectile;
    }

    public override void OnAcquire(UnitBase unit)
    {
        unit.EquipProjectile(this);
    }

    public override string GetTooltipText(int shopPrice)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(description);
        sb.AppendLine();

        sb.AppendLine($"Type: {slotType}");
        sb.AppendLine($"Price: {shopPrice}");

        if (projectile == null)
            return sb.ToString();

        sb.AppendLine();
        sb.AppendLine("<b>Projectile Stats</b>");

        AddStat(sb, "Health", ProjectileStatType.MaxHealth);
        AddStat(sb, "Shield", ProjectileStatType.StartingShield);
        AddStat(sb, "Collision Damage", ProjectileStatType.CollisionDamage);
        AddStat(sb, "Knockback", ProjectileStatType.CollisionKnockback);
        AddStat(sb, "Mass", ProjectileStatType.Mass);
        return sb.ToString();
    }

    private void AddStat(StringBuilder sb, string label, ProjectileStatType stat)
    {
        float value = projectile.GetBaseStat(stat);

        if (Mathf.Approximately(value, 0f))
            return;

        sb.AppendLine($"{label}: {value}");
    }
}