using UnityEngine;

public static class DamagePresets
{
    // -------------------------
    // BASIC CATEGORIES
    // -------------------------

    public static DamageInfo Generic(float amount)
    {
        return new DamageInfo(
            amount,
            DamageCategory.Generic,
            DamageElement.None
        );
    }

    public static DamageInfo Collision(float amount)
    {
        return new DamageInfo(
            amount,
            DamageCategory.Collision,
            DamageElement.Kinetic
        );
    }

    public static DamageInfo Explosion(float amount, DamageElement element = DamageElement.Fire)
    {
        return new DamageInfo(
            amount,
            DamageCategory.Explosion,
            element
        );
    }

    public static DamageInfo Environmental(float amount, DamageElement element = DamageElement.None)
    {
        return new DamageInfo(
            amount,
            DamageCategory.Environmental,
            element
        );
    }

    public static DamageInfo True(float amount)
    {
        return new DamageInfo(
            amount,
            DamageCategory.True,
            DamageElement.None
        )
        {
            BypassShields = true,
            BypassResistance = true
        };
    }

    public static DamageInfo Burn(float amount)
    {
        return new DamageInfo(
            amount,
            DamageCategory.DamageOverTime,
            DamageElement.Fire
        )
        {
            BypassShields = true
        };
    }

    public static DamageInfo Poison(float amount)
    {
        return new DamageInfo(
            amount,
            DamageCategory.DamageOverTime,
            DamageElement.Poison
        )
        {
            BypassShields = true
        };
    }

    public static DamageInfo ElectricDOT(float amount)
    {
        return new DamageInfo(
            amount,
            DamageCategory.DamageOverTime,
            DamageElement.Electric
        )
        {
            BypassShields = true
        };
    }
}