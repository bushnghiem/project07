using UnityEngine;

public enum StatusEffectApplicationPolicy
{
    Refresh,
    Independent
}

public abstract class StatusEffectData : ScriptableObject
{
    public string effectID;
    public string displayName;

    [Header("UI")]
    public Sprite statusIcon;

    [Header("Application")]
    public StatusEffectApplicationPolicy applicationPolicy =
    StatusEffectApplicationPolicy.Refresh;

    public bool isStackable;
    public int maxStacks = 1;
    public int duration; // in turns

    [TextArea(3, 6)]
    public string description;

    public abstract StatusEffectInstance CreateInstance(Unit target);
}
