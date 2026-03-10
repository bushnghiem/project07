using UnityEngine;

[CreateAssetMenu(menuName = "Items/Projectile Item")]
public class ProjectileItem : Item
{
    public Projectile projectile;

    public override void OnAcquire(UnitBase unit)
    {
        unit.EquipProjectile(projectile);
    }
}
