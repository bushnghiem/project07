using UnityEngine;

public enum RewardType
{
    Currency,
    Keys,
    Item,
    Ship,
    HealAllPlayers
}

public enum RewardValueMode
{
    Fixed,
    RandomRange
}

public enum RewardItemMode
{
    Fixed,
    RandomFromFloorPool
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

    [Header("Item")]
    public RewardItemMode itemMode = RewardItemMode.Fixed;

    public Item item;

    [Header("Ship")]
    public ShipRunData ship;
}
