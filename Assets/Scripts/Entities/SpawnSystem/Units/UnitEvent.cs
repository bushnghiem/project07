using UnityEngine;

public struct UnitEvent
{
    public UnitBase source;   // who caused it
    public UnitBase target;   // who received it (optional)
    public UnitEventType type;
    public float value;
}
