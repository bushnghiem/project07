using UnityEngine;
using System.Collections.Generic;

public class TeamManager : MonoBehaviour
{
    public List<ShipMetaData> availableShips;
    public List<ShipMetaData> selectedTeam;

    public void SelectShip(ShipMetaData ship)
    {
        if (!selectedTeam.Contains(ship))
            selectedTeam.Add(ship);
    }
}
