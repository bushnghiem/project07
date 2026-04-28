using UnityEngine;

public abstract class StatusEffectData : ScriptableObject
{
    public string effectID;
    public string displayName;

    public bool isStackable;
    public int maxStacks = 1;
    public int duration; // in turns

    public abstract StatusEffectInstance CreateInstance(Unit target);
}
