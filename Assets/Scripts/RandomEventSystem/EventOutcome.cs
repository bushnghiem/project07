using UnityEngine;

public enum OutcomeType
{
    GainCurrency,
    LoseCurrency,
    GainKeys,
    LoseKeys,
    StartCombat,
    HealPlayer,
    DamagePlayer,
    GiveItem,
    GiveCharges,
    TakeTime,
    StartQuest,
    Nothing
}

[System.Serializable]
public class EventOutcome
{
    public OutcomeType type;

    public TileModification tileModification;

    public int value;

    public DamageDefinition damage;

    public EncounterData encounter;
    public Item item;

    public QuestData quest;


    [Range(0f, 1f)]
    public float chance = 1f;
}