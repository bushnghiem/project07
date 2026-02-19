using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject[] unitPrefab;
    public Transform[] spawnPoints;
    public ShipRunData enemyData;

    public void SpawnUnit(ShipRunData ShipRunData, int spawnIndex)
    {
        GameObject obj = Instantiate(unitPrefab[1], spawnPoints[spawnIndex].position, Quaternion.identity);
        obj.GetComponent<Unit>().Initialize(ShipRunData);

    }

    public void SpawnTeam(RunData runData)
    {
        for (int i = 0; i < runData.team.Count; i++)
        {
            GameObject obj = Instantiate(unitPrefab[0], spawnPoints[i].position, Quaternion.identity);

            obj.GetComponent<Unit>().Initialize(runData.team[i]);
        }
    }

}
