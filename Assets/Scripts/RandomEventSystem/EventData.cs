using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/Event Data")]
public class EventData : ScriptableObject
{
    [Header("Identification")]
    public string eventName;

    [SerializeField]
    private string eventId;

    public string EventId => eventId;

    [TextArea(3, 6)]
    public string description;

    public List<EventOption> options = new List<EventOption>();

    [Header("Spawn Rules")]
    public int minFloor;
    public int maxFloor;
    public bool unique;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(eventId))
        {
            eventId = System.Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif
}
