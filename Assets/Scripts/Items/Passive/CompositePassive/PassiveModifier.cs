using UnityEngine;

public abstract class PassiveModifier : ScriptableObject
{
    public abstract void Apply(
        UnitBase unit,
        PassiveItemInstance instance);

    public abstract void Remove(
        UnitBase unit,
        PassiveItemInstance instance);
}
