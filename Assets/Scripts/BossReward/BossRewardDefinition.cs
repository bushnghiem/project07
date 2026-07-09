using UnityEngine;

public enum BossRewardType
{
    Currency,
    Keys,
    Item,
    Ship,
    HealAllPlayers
}

[CreateAssetMenu(menuName = "Scriptable Objects/Boss Reward")]
public class BossRewardDefinition : ScriptableObject
{
    [Header("Display")]
    public string rewardName;

    [TextArea]
    public string description;

    public Sprite icon;

    [Header("Reward")]
    public BossRewardType rewardType;

    public int value;

    public Item item;

    public ShipRunData ship;
}