using UnityEngine;

public static class DamageStatUtility
{
    public static ShipStatType? GetResistanceStat(DamageCategory category)
    {
        return category switch
        {
            DamageCategory.Collision => ShipStatType.CollisionResistance,
            DamageCategory.Explosion => ShipStatType.ExplosionResistance,
            DamageCategory.DamageOverTime => ShipStatType.DotResistance,
            _ => null
        };
    }

    public static ShipStatType? GetResistanceStat(DamageElement element)
    {
        return element switch
        {
            DamageElement.Fire => ShipStatType.FireResistance,
            DamageElement.Electric => ShipStatType.ElectricResistance,
            DamageElement.Ice => ShipStatType.IceResistance,
            DamageElement.Poison => ShipStatType.PoisonResistance,
            _ => null
        };
    }
}