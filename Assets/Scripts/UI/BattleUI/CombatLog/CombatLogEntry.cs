using UnityEngine;
using System;

[Serializable]
public class CombatLogEntry
{
    public string Message;
    public DateTime Timestamp;

    public CombatLogEntry(string message)
    {
        Message = message;
        Timestamp = DateTime.Now;
    }
}