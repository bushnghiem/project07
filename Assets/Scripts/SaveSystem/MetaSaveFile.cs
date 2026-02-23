using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MetaSaveFile
{
    public int version = 1;

    public List<ShipMetaData> ships = new();
    public int playerCurrency;
    public int totalWins;
    public int totalRuns;
}