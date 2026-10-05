using UnityEngine;
using System.Collections.Generic;

public class StatBreakdown
{
    public ShipStatType statType;

    public float baseValue;
    public float finalValue;

    public List<StatBreakdownEntry> entries = new();
}

