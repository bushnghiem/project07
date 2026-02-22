using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveFile
{
    public int version = 1;

    public RunData currentRun;
    public List<ShipMetaData> metaShips;

    public int playerCurrency;
}