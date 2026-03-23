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
        unit.EquipProjectile(projectile);
    }
}