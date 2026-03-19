using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/Event Data")]
public class EventData : ScriptableObject
{
    public string eventName;

    [TextArea(3, 6)]
    public string description;

    public List<EventOption> options = new List<EventOption>();

    [Header("Spawn Rules")]
    public int minFloor;
    public int maxFloor;
    public bool unique;
}
