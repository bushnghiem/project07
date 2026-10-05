using UnityEngine;
using System.Collections.Generic;

public class StatBreakdownEntry
{
    public string sourceName;

    public StatModifierOperation operation;

    public float value;

    public StatBreakdownEntry(
        string sourceName,
        StatModifierOperation operation,
        float value)
    {
        this.sourceName = sourceName;
        this.operation = operation;
        this.value = value;
    }
}


