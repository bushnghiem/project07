using UnityEngine;
using System.Collections.Generic;

public class UnitSpawner : MonoBehaviour
{
    public GameObject playerUnitPrefab;
    public GameObject enemyUnitPrefab;
    public Transform playerAnchor;
    public Transform enemyAnchor;

    public void SetAnchorPositions(Vector3 playerAnchorPos, Vector3 enemyAnchorPos)
    {
        playerAnchor.position = playerAnchorPos;
        enemyAnchor.position = enemyAnchorPos;
    }

    private void SpawnTeam(List<ShipRunData> units, FormationData formation, GameObject prefab, Transform anchor)
    {
        for (int i = 0; i < units.Count; i++)
        {
            Vector3 spawnPos = anchor.position;

            if (i < formation.positions.Count)
                spawnPos = anchor.TransformPoint(formation.positions[i]);

            GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);
            obj.GetComponent<UnitBase>().Initialize(units[i]);
        }
    }

    public void SpawnEnemyTeam(List<ShipRunData> enemies, FormationData formation)
    {
        SpawnTeam(enemies, formation, enemyUnitPrefab, enemyAnchor);
    }

    public void SpawnPlayerTeam(List<ShipRunData> players, FormationData formation)
    {
        SpawnTeam(players, formation, playerUnitPrefab, playerAnchor);
    }

}
