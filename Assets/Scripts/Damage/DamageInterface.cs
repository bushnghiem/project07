using UnityEngine;

public enum DamageCategory
{
    Generic,

    Collision,
    Explosion,

    DamageOverTime,

    Environmental,

    True
}

public enum DamageElement
{
    None,

    Kinetic,
    Fire,
    Electric,
    Ice,
    Poison,

    Plasma,
    Radiation
}

public struct DamageInfo
{
    public float Amount;

    public DamageCategory Category;
    public DamageElement Element;

    // Who gets credit for the damage.
    public UnitBase Instigator;

    // What actually dealt the damage.
    public Entity Source;

    public bool BypassShields;
    public bool BypassResistance;

    public Vector3 HitPoint;

    public DamageInfo(
        float amount,
        DamageCategory category = DamageCategory.Generic,
        DamageElement element = DamageElement.None,
        UnitBase instigator = null,
        Entity source = null
    )
    {
        Amount = amount;

        Category = category;
        Element = element;

        Instigator = instigator;
        Source = source;

        BypassShields = false;
        BypassResistance = false;

        HitPoint = Vector3.zero;
    }
}