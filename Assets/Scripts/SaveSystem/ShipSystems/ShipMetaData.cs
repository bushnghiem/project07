using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class ShipMetaData
{
    public string templateID;

    public bool isUnlocked;

    public HashSet<int> unlockedColorIndices = new();

    public int totalWins;
    public int totalRuns;
}

