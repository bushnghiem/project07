using UnityEngine;
using System;

[Serializable]
public class StatModifier
{
    public ShipStatType statType;

    public float flatBonus;
    public float percentBonus;

    public float Apply(float baseValue)
    {
        float value = baseValue + flatBonus;
        value += value * percentBonus;
        return value;
    }
}
