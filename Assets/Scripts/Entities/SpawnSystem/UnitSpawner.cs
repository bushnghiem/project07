using UnityEngine;
using System.Collections.Generic;

public class UnitSpawner : MonoBehaviour
{
    public GameObject playerUnitPrefab;
    public GameObject enemyUnitPrefab;

    public Transform playerAnchor;
    public Transform enemyAnchor;

    public AIDatabase aiDatabase;

    public void SetAnchorPositions(
        Vector3 playerAnchorPos,
        Vector3 enemyAnchorPos)
    {
        playerAnchor.position = playerAnchorPos;
        enemyAnchor.position = enemyAnchorPos;
    }

    // Existing player spawning remains preset-based.
    public void SpawnPlayerTeam(
        List<ShipRunData> players,
        FormationData formation)
    {
        SpawnPresetTeam(
            players,
            formation,
            playerUnitPrefab,
            playerAnchor
        );
    }

    private void SpawnPresetTeam(
        List<ShipRunData> units,
        FormationData formation,
        GameObject prefab,
        Transform anchor)
    {
        if (units == null || prefab == null || anchor == null)
            return;

        for (int i = 0; i < units.Count; i++)
        {
            Vector3 position = anchor.position;

            if (formation != null &&
                formation.positions != null &&
                i < formation.positions.Count)
            {
                position = anchor.TransformPoint(
                    formation.positions[i]
                );
            }

            SpawnUnit(units[i], prefab, position);
        }
    }

    public void SpawnEnemyTeam(
        List<ShipRunData> enemies,
        FormationData formation,
        EnemySpawnMode mode,
        EnemySpawnSettings settings,
        Vector3 combatCenter,
        int seed)
    {
        if (enemies == null || enemyUnitPrefab == null)
            return;

        switch (mode)
        {
            case EnemySpawnMode.Preset:
                SpawnPresetTeam(
                    enemies,
                    formation,
                    enemyUnitPrefab,
                    enemyAnchor
                );
                break;

            case EnemySpawnMode.RandomAnchor:
                SpawnRandomAnchorFormation(
                    enemies,
                    formation,
                    settings,
                    combatCenter,
                    seed
                );
                break;

            case EnemySpawnMode.FullyRandom:
                SpawnFullyRandom(
                    enemies,
                    settings,
                    combatCenter,
                    playerAnchor.position,
                    seed
                );
                break;
        }
    }

    private void SpawnRandomAnchorFormation(
        List<ShipRunData> enemies,
        FormationData formation,
        EnemySpawnSettings settings,
        Vector3 center,
        int seed)
    {
        if (enemyAnchor == null)
            return;

        if (settings == null)
            settings = new EnemySpawnSettings();

        System.Random rng = new System.Random(seed);

        // Pick a deterministic random anchor position.
        Vector3 halfSize = settings.anchorAreaSize * 0.5f;

        Vector3 offset = new Vector3(
            Range(rng, -halfSize.x, halfSize.x),
            Range(rng, -halfSize.y, halfSize.y),
            Range(rng, -halfSize.z, halfSize.z)
        );

        enemyAnchor.position = center + offset;

        // Keep the formation's relative ship positions.
        SpawnPresetTeam(
            enemies,
            formation,
            enemyUnitPrefab,
            enemyAnchor
        );
    }


    private void SpawnFullyRandom(
        List<ShipRunData> enemies,
        EnemySpawnSettings settings,
        Vector3 center,
        Vector3 playerPosition,
        int seed)
    {
        if (settings == null)
            settings = new EnemySpawnSettings();

        System.Random rng = new System.Random(seed);

        Vector3 halfSize = settings.randomAreaSize * 0.5f;

        List<Vector3> spawnedPositions = new List<Vector3>();

        // Number of attempts used to find a good position.
        const int maxAttempts = 30;

        float desiredSpacing = Mathf.Max(
            0f,
            settings.minDistanceBetweenShips
        );

        float playerExclusionRadius = Mathf.Max(
            0f,
            settings.minDistanceFromPlayer
        );

        for (int i = 0; i < enemies.Count; i++)
        {
            Vector3 position = center;
            bool foundPosition = false;

            // Try to find a valid position.
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                position = center + new Vector3(
                    Range(rng, -halfSize.x, halfSize.x),
                    Range(rng, -halfSize.y, halfSize.y),
                    Range(rng, -halfSize.z, halfSize.z)
                );

                // Check distance from player anchor.
                if (Vector3.Distance(position, playerPosition)
                    < playerExclusionRadius)
                {
                    continue;
                }

                // Check distance from other enemies.
                bool tooClose = false;

                foreach (Vector3 existing in spawnedPositions)
                {
                    if (Vector3.Distance(position, existing)
                        < desiredSpacing)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (tooClose)
                    continue;

                foundPosition = true;
                break;
            }

            // Fallback: guarantee that the enemy always spawns.
            if (!foundPosition)
            {
                position = center + new Vector3(
                    Range(rng, -halfSize.x, halfSize.x),
                    Range(rng, -halfSize.y, halfSize.y),
                    Range(rng, -halfSize.z, halfSize.z)
                );

                Debug.LogWarning(
                    $"Enemy {i} required a fallback spawn position."
                );
            }

            spawnedPositions.Add(position);

            SpawnUnit(
                enemies[i],
                enemyUnitPrefab,
                position
            );
        }
    }


    private void SpawnUnit(
        ShipRunData data,
        GameObject prefab,
        Vector3 position)
    {
        GameObject obj = Instantiate(
            prefab,
            position,
            Quaternion.identity
        );

        UnitBase unit = obj.GetComponent<UnitBase>();

        if (unit == null)
        {
            Debug.LogError(
                $"Prefab {prefab.name} has no UnitBase component."
            );

            Destroy(obj);
            return;
        }

        unit.Initialize(data);

        Enemy enemy = unit as Enemy;

        if (enemy != null)
        {
            enemy.aiController.InitializeFromRunData(
                data,
                aiDatabase
            );
        }
    }

    private float Range(
        System.Random rng,
        float min,
        float max)
    {
        return min + (float)rng.NextDouble() * (max - min);
    }
}