using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/Event Pool")]
public class EventPool : ScriptableObject
{
    public List<EventData> events;
}