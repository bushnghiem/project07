using UnityEngine;

public abstract class PassiveItem : ScriptableObject
{
    public string passiveItemID;
    public string itemName;
    public Sprite icon;

    public string description;

    // Apply effect to a unit (called at start of run, or when equipped)
    public abstract void ApplyEffect(Unit unit);

    // Remove effect if needed (for temporary items)
    public virtual void RemoveEffect(Unit unit) { }
}
