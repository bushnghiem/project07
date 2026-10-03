using UnityEngine;

public class StatBreakdownEntry
{
    public string SourceName;
    public float FlatBonus;
    public float PercentBonus;

    public bool IsBase =>
        SourceName == "Base";
}

