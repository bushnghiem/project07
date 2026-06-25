using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EventOption
{
    public string optionText;

    public List<EventCondition> conditions;

    [Header("Outcome Groups")]
    public List<OutcomeGroup> outcomeGroups = new List<OutcomeGroup>();

    public bool removeAfterUse;
}