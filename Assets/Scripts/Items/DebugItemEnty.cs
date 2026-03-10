using System;
using UnityEngine;

[Serializable]
public class DebugItemEntry
{
    public string itemType;      // "Passive", "Active", or "Projectile"
    public string itemID;
    public string itemName;
    public string description;
}