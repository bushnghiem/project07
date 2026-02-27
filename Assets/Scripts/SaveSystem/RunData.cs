using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RunData
{
    public int runSeed;
    public Vector2Int currentGridPosition;
    public EncounterData currentEncounter;
    public List<ShipRunData> team = new List<ShipRunData>();
    public int runCurrency = 0;

    public List<Vector2Int> clearedCombatTiles = new List<Vector2Int>();
}
