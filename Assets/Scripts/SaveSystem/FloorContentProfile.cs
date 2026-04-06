using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/Floor Content")]
public class FloorContentProfile : ScriptableObject
{
    [Header("Identification")]
    public string floorName;

    [Header("Weighted Random Selection")]
    [Range(1, 100)]
    public int weight = 10;


    [Header("Shop")]
    public List<Item> shopItems;

    [Header("Encounters")]
    public List<EncounterData> combatEncounters;
    public List<EncounterData> corruptionEncounters;

    [Header("Events")]
    public List<EventData> events;
}