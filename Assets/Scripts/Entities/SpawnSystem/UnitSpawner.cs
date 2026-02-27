using UnityEngine;
using System.Collections.Generic;

public class UnitSpawner : MonoBehaviour
{
    public GameObject playerUnitPrefab;
    public GameObject enemyUnitPrefab;
    public Transform playerAnchor;
    public Transform enemyAnchor;
    public ShipRunData enemyData;

    /*
    public void SpawnUnit(ShipRunData ShipRunData, int spawnIndex)
    {
        GameObject obj = Instantiate(enemyUnitPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
        obj.GetComponent<Unit>().Initialize(ShipRunData);

    }

    public void SpawnTeam(RunData runData)
    {
        for (int i = 0; i < runData.team.Count; i++)
        {
            GameObject obj = Instantiate(playerUnitPrefab, spawnPoints[i].position, Quaternion.identity);

            obj.GetComponent<Unit>().Initialize(runData.team[i]);
        }
    }
    */

    public void SpawnEnemyTeam(List<ShipRunData> enemies, FormationData formation)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Vector3 spawnPos = enemyAnchor.position;

            if (i < formation.positions.Count)
                spawnPos += (Vector3)formation.positions[i];

            GameObject obj = Instantiate(enemyUnitPrefab, spawnPos, Quaternion.identity);
            obj.GetComponent<Unit>().Initialize(enemies[i]);
            Debug.Log("Spawned Enemy");
        }
    }

    public void SpawnPlayerTeam(List<ShipRunData> players, FormationData formation)
    {
        for (int i = 0; i < players.Count; i++)
        {
            Vector3 spawnPos = playerAnchor.position;

            if (i < formation.positions.Count)
                spawnPos += (Vector3)formation.positions[i];

            GameObject obj = Instantiate(playerUnitPrefab, spawnPos, Quaternion.identity);
            obj.GetComponent<Unit>().Initialize(players[i]);
            Debug.Log("Spawned Player");
        }
    }

}
