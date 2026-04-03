using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RunData
{
    public int runSeed;

    public int currentFloor = 0;

    public FloorData currentFloorData;
    public List<FloorData> completedFloors = new List<FloorData>();

    public FormationData playerFormation;
    public List<ShipRunData> team = new List<ShipRunData>();

    public int runCurrency = 0;
}
