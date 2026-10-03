using UnityEngine;
using System.Collections.Generic;

public class StatBreakdown
{
    public ShipStatType StatType;

    public float BaseValue;
    public float FinalValue;

    public List<StatBreakdownEntry> Entries = new();
}

