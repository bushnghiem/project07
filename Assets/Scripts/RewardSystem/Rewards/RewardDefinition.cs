using UnityEngine;

public enum RewardType
{
    Currency,
    Keys,
    Item,
    Ship,
    HealAllPlayers
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

    public int value;

    public Item item;

    public ShipRunData ship;
}