using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/Floor Content")]
public class FloorContentProfile : ScriptableObject
{
    [Header("Shop")]
    public List<Item> shopItems;

    [Header("Encounters")]
    public List<EncounterData> combatEncounters;
    public List<EncounterData> corruptionEncounters;

    [Header("Events")]
    public List<EventData> events;
}