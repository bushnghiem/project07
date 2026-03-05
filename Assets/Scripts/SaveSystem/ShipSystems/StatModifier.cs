using UnityEngine;
using System;

[Serializable]
public class StatModifier
{
    public ShipStatType statType;

    public float flatBonus;
    public float percentBonus;

    public object source;
}
