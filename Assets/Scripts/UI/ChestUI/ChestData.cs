using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ChestData
{
    public Vector2Int gridPosition;
    public bool opened;

    public List<string> rewardItemIDs = new();
}