using UnityEngine;
using System.Collections.Generic;

public class UnitSpawner : MonoBehaviour
{
    public GameObject playerUnitPrefab;
    public GameObject enemyUnitPrefab;
    public Transform playerAnchor;
    public Transform enemyAnchor;

    public AIDatabase aiDatabase;

    public void SetAnchorPositions(Vector3 playerAnchorPos, Vector3 enemyAnchorPos)
    {
        playerAnchor.position = playerAnchorPos;
        enemyAnchor.position = enemyAnchorPos;
    }

    private void SpawnTeam(List<ShipRunData> units, FormationData formation, GameObject prefab, Transform anchor)
    {
        Debug.Log($"units: {units}");
        Debug.Log($"formation: {formation}");
        Debug.Log($"prefab: {prefab}");
        Debug.Log($"anchor: {anchor}");
        for (int i = 0; i < units.Count; i++)
        {
            Vector3 spawnPos = anchor.position;

            if (i < formation.positions.Count)
                spawnPos = anchor.TransformPoint(formation.positions[i]);

            GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);

            Debug.Log(obj);
            UnitBase unit = obj.GetComponent<UnitBase>();
            unit.Initialize(units[i]);

            Debug.Log(unit);

            Enemy enemy = unit as Enemy;
            if (enemy != null)
            {
                Debug.Log(enemy.aiController);
                Debug.Log(aiDatabase);
                enemy.aiController.InitializeFromRunData(units[i],aiDatabase);
            }
        }
    }

    public void SpawnEnemyTeam(List<ShipRunData> enemies, FormationData formation)
    {
        SpawnTeam(enemies, formation, enemyUnitPrefab, enemyAnchor);
    }

    public void SpawnPlayerTeam(List<ShipRunData> players, FormationData formation)
    {
        SpawnTeam(players, formation, playerUnitPrefab, playerAnchor);
        Debug.Log("Spawned Players");
    }

}
