using UnityEngine;
using System;

[Serializable]
public class StatAdjustment
{
    public ShipStatType statType;

    public StatModifierOperation operation;

    public float value;
}

