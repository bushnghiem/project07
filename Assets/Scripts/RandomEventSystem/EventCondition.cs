using UnityEngine;

public enum ConditionType
{
    HasCurrency,
    LowHealth,
    HasShip,
    HasItem
}

[System.Serializable]
public class EventCondition
{
    public ConditionType type;
    public int value;
}