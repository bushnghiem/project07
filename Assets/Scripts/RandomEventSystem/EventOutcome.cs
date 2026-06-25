using UnityEngine;

public enum OutcomeType
{
    GainCurrency,
    LoseCurrency,
    StartCombat,
    HealPlayer,
    DamagePlayer,
    GiveItem,
    TakeTime,
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

    [Range(0f, 1f)]
    public float chance = 1f;
}