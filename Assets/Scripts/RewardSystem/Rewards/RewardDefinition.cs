using UnityEngine;

public enum RewardType
{
    Currency,
    Keys,
    Item,
    RandomItem,
    Ship,
    HealAllPlayers
}


public enum RewardValueMode
{
    Fixed,
    RandomRange
}

[CreateAssetMenu(menuName = "Scriptable Objects/Reward")]
public class RewardDefinition : ScriptableObject
{
    [Header("Display")]
    public string rewardName;

    [TextArea]
    public string description;

    public Sprite icon;

    [Header("Reward")]
    public RewardType rewardType;

    [Header("Value")]
    public RewardValueMode valueMode = RewardValueMode.Fixed;

    public int value;
    public int minValue;
    public int maxValue;

    public Item item;

    [Header("Ship")]
    public ShipRunData ship;
}
