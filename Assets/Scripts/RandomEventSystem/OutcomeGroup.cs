using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class OutcomeGroup
{
    [Header("UI")]
    public string groupName;

    [TextArea(2, 4)]
    public string description;

    [Range(0f, 1f)]
    public float groupChance = 1f;

    [Header("Outcomes")]
    public List<EventOutcome> outcomes = new List<EventOutcome>();
}