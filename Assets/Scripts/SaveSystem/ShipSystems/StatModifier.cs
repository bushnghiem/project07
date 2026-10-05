using System;
using UnityEngine;

public enum StatModifierOperation
{
    Flat,
    PercentAdd,
    PercentMultiply,
    Override
}

[Serializable]
public class StatModifier
{
    public ShipStatType statType;

    public StatModifierOperation operation;

    public float value;

    // Used internally to identify/remove the modifier.
    public string sourceID;

    // Used by the UI.
    public string sourceName;

    public StatModifier(
        ShipStatType statType,
        StatModifierOperation operation,
        float value,
        string sourceID,
        string sourceName)
    {
        this.statType = statType;
        this.operation = operation;
        this.value = value;
        this.sourceID = sourceID;
        this.sourceName = sourceName;
    }
}
