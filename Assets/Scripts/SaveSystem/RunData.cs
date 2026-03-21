using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RunData
{
    public int runSeed;
    public Vector2Int currentGridPosition;
    public EncounterData currentEncounter;
    public FormationData playerFormation;
    public List<ShipRunData> team = new List<ShipRunData>();
    public int runCurrency = 0;
    public int stepsTaken = 0;

    public List<Vector2Int> clearedCombatTiles = new List<Vector2Int>();
    public List<Vector2Int> clearedEventTiles = new List<Vector2Int>();
    public Dictionary<Vector2Int, List<int>> usedEventOptions = new Dictionary<Vector2Int, List<int>>();
    public List<ShopData> shops = new List<ShopData>();
}
