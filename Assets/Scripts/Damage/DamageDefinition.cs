using UnityEngine;

[System.Serializable]
public class DamageDefinition
{
    public float amount;

    public DamageCategory category;

    public DamageElement element;

    public bool bypassShields;

    public bool bypassResistance;

    public DamageInfo ToDamageInfo()
    {
        return new DamageInfo(
            amount,
            category,
            element
        )
        {
            BypassShields = bypassShields,
            BypassResistance = bypassResistance
        };
    }
}