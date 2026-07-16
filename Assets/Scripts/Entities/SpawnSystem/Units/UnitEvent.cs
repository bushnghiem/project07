using UnityEngine;

public struct UnitEvent
{
    public UnitBase source;      // Who gets credit
    public Entity damageSource;  // What actually dealt the damage

    public UnitBase target;

    public UnitEventType type;
    public float value;
}
