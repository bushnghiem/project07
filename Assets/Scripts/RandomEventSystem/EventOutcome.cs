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

    // Optional references depending on type
    public EncounterData encounter;
    public Item item;
}