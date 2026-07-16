using UnityEngine;

public static class DamagePresets
{
    public static DamageInfo Generic(
        float amount,
        UnitBase instigator = null,
        Entity source = null)
    {
        return new DamageInfo(
            amount,
            DamageCategory.Generic,
            DamageElement.None,
            instigator,
            source
        );
    }

    public static DamageInfo Collision(
        float amount,
        UnitBase instigator = null,
        Entity source = null)
    {
        return new DamageInfo(
            amount,
            DamageCategory.Collision,
            DamageElement.Kinetic,
            instigator,
            source
        );
    }

    public static DamageInfo Explosion(
        float amount,
        UnitBase instigator = null,
        Entity source = null,
        DamageElement element = DamageElement.Fire)
    {
        return new DamageInfo(
            amount,
            DamageCategory.Explosion,
            element,
            instigator,
            source
        );
    }

    public static DamageInfo Environmental(
        float amount,
        UnitBase instigator = null,
        Entity source = null,
        DamageElement element = DamageElement.None)
    {
        return new DamageInfo(
            amount,
            DamageCategory.Environmental,
            element,
            instigator,
            source
        );
    }

    public static DamageInfo True(
        float amount,
        UnitBase instigator = null,
        Entity source = null)
    {
        return new DamageInfo(
            amount,
            DamageCategory.True,
            DamageElement.None,
            instigator,
            source
        )
        {
            BypassShields = true,
            BypassResistance = true
        };
    }

    public static DamageInfo Burn(
        float amount,
        UnitBase instigator = null,
        Entity source = null)
    {
        return new DamageInfo(
            amount,
            DamageCategory.DamageOverTime,
            DamageElement.Fire,
            instigator,
            source
        )
        {
            BypassShields = true
        };
    }

    public static DamageInfo Poison(
        float amount,
        UnitBase instigator = null,
        Entity source = null)
    {
        return new DamageInfo(
            amount,
            DamageCategory.DamageOverTime,
            DamageElement.Poison,
            instigator,
            source
        )
        {
            BypassShields = true
        };
    }

    public static DamageInfo ElectricDOT(
        float amount,
        UnitBase instigator = null,
        Entity source = null)
    {
        return new DamageInfo(
            amount,
            DamageCategory.DamageOverTime,
            DamageElement.Electric,
            instigator,
            source
        )
        {
            BypassShields = true
        };
    }
}