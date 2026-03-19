using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EventOption
{
    public string optionText;

    public List<EventCondition> conditions;
    public List<EventOutcome> outcomes;

    public bool removeAfterUse;
}
