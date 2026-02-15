using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject unitPrefab;
    public Transform[] spawnPoints;

    public void SpawnUnit(ShipRunData ShipRunData)
    {
        GameObject unit = Instantiate(unitPrefab);
        Unit unitComponent = unit.GetComponent<Unit>();

        unitComponent.Initialize(ShipRunData);
    }

    public void SpawnTeam(RunData runData)
    {
        for (int i = 0; i < runData.team.Count; i++)
        {
            GameObject obj = Instantiate(unitPrefab, spawnPoints[i].position, Quaternion.identity);

            obj.GetComponent<Unit>().Initialize(runData.team[i]);
        }
    }

}
