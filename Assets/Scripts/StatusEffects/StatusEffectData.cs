using UnityEngine;

public abstract class StatusEffectData : ScriptableObject
{
    public string effectID;
    public string displayName;

    [Header("UI")]
    public Sprite statusIcon;

    public bool isStackable;
    public int maxStacks = 1;
    public int duration; // in turns

    [TextArea(3, 6)]
    public string description;

    public abstract StatusEffectInstance CreateInstance(Unit target);
}
