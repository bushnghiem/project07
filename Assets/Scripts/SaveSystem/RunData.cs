using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RunData
{
    public int runSeed;
    public Vector2Int currentGridPosition;
    public List<ShipRunData> team = new List<ShipRunData>();
}
