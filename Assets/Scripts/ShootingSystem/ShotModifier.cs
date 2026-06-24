using UnityEngine;

public abstract class ShotModifier : ScriptableObject
{
    public abstract void Modify(
        ShotPattern pattern,
        UnitBase shooter
    );
}