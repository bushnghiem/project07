using UnityEngine;
using System.Collections.Generic;

public class EnvironmentSpawner : MonoBehaviour
{
    [SerializeField]
    private LayerMask shipLayers;

    [SerializeField]
    private LayerMask environmentLayers;

    public void SpawnEnvironment(
        EnvironmentLayout layout,
        Vector3 combatCenter,
        int seed)
    {
        if (layout == null)
            return;

        switch (layout.spawnMode)
        {
            case EnvironmentSpawnMode.Preset:
                SpawnPresetEnvironment(
                    layout,
                    combatCenter
                );
                break;

            case EnvironmentSpawnMode.Random:
                SpawnRandomEnvironment(
                    layout.randomSettings,
                    combatCenter,
                    seed
                );
                break;
        }
    }

    private void SpawnPresetEnvironment(
        EnvironmentLayout layout,
        Vector3 combatCenter)
    {
        foreach (var objData in layout.environmentObjects)
        {
            Vector3 spawnPosition =
                combatCenter + objData.position;

            GameObject obj = Instantiate(
                objData.prefab,
                spawnPosition,
                Quaternion.Euler(objData.rotation)
            );

            obj.transform.localScale = objData.scale;
        }
    }

    private void SpawnRandomEnvironment(
        RandomEnvironmentSettings settings,
        Vector3 combatCenter,
        int seed)
    {
        if (settings == null)
        {
            Debug.LogWarning("Random environment settings are missing.");
            return;
        }

        if (settings.prefabs == null || settings.prefabs.Count == 0)
        {
            Debug.LogWarning("Random environment has no prefabs.");
            return;
        }

        System.Random rng = new System.Random(seed);

        int spawnedCount = 0;

        for (int i = 0; i < settings.objectCount; i++)
        {
            bool spawned = TrySpawnRandomObject(
                settings,
                combatCenter,
                rng
            );

            if (spawned)
                spawnedCount++;
        }
    }

    private bool TrySpawnRandomObject(
        RandomEnvironmentSettings settings,
        Vector3 combatCenter,
        System.Random rng)
    {
        for (int attempt = 0;
             attempt < settings.maxAttemptsPerObject;
             attempt++)
        {
            RandomEnvironmentPrefab prefabData =
                GetWeightedRandomPrefab(
                    settings.prefabs,
                    rng
                );

            if (prefabData == null ||
                prefabData.prefab == null)
            {
                continue;
            }

            Vector3 position =
                GetRandomPosition(
                    combatCenter,
                    settings,
                    rng
                );

            bool positionClear =
                IsPositionClear(
                    position,
                    prefabData.collisionRadius,
                    settings.minDistanceFromShips,
                    settings.minDistanceFromEnvironment
                );

            if (!positionClear)
                continue;

            Quaternion rotation =
                Quaternion.Euler(
                    0f,
                    GetRandomFloat(
                        rng,
                        0f,
                        360f
                    ),
                    0f
                );

            GameObject obj = Instantiate(
                prefabData.prefab,
                position,
                rotation
            );

            obj.transform.localScale =
                GetRandomScale(
                    prefabData.minScale,
                    prefabData.maxScale,
                    rng
                );

            return true;
        }

        return false;
    }

    private Vector3 GetRandomPosition(
        Vector3 center,
        RandomEnvironmentSettings settings,
        System.Random rng)
    {
        float angle =
            GetRandomFloat(
                rng,
                0f,
                Mathf.PI * 2f
            );

        float distance =
            GetRandomFloat(
                rng,
                settings.minDistanceFromCenter,
                settings.maxDistanceFromCenter
            );

        Vector3 offset = new Vector3(
            Mathf.Cos(angle) * distance,
            0f,
            Mathf.Sin(angle) * distance
        );

        return center + offset;
    }

    private bool IsPositionClear(
        Vector3 position,
        float collisionRadius,
        float minDistanceFromShips,
        float minDistanceFromEnvironment)
    {
        Collider[] ships = Physics.OverlapSphere(
            position,
            collisionRadius + minDistanceFromShips,
            shipLayers
        );

        if (ships.Length > 0)
            return false;

        Collider[] environment =
            Physics.OverlapSphere(
                position,
                collisionRadius +
                minDistanceFromEnvironment,
                environmentLayers
            );

        if (environment.Length > 0)
            return false;

        return true;
    }

    private RandomEnvironmentPrefab
        GetWeightedRandomPrefab(
            List<RandomEnvironmentPrefab> prefabs,
            System.Random rng)
    {
        float totalWeight = 0f;

        foreach (var prefab in prefabs)
        {
            if (prefab == null || prefab.prefab == null)
                continue;

            if (prefab.weight <= 0f)
                continue;

            totalWeight += prefab.weight;
        }

        if (totalWeight <= 0f)
            return null;

        float randomValue =
            GetRandomFloat(
                rng,
                0f,
                totalWeight
            );

        float currentWeight = 0f;

        foreach (var prefab in prefabs)
        {
            if (prefab == null || prefab.prefab == null)
                continue;

            if (prefab.weight <= 0f)
                continue;

            currentWeight += prefab.weight;

            if (randomValue <= currentWeight)
                return prefab;
        }

        return null;
    }

    private Vector3 GetRandomScale(
        Vector3 min,
        Vector3 max,
        System.Random rng)
    {
        return new Vector3(
            GetRandomFloat(rng, min.x, max.x),
            GetRandomFloat(rng, min.y, max.y),
            GetRandomFloat(rng, min.z, max.z)
        );
    }

    private float GetRandomFloat(
        System.Random rng,
        float min,
        float max)
    {
        return (float)(
            min +
            (max - min) * rng.NextDouble()
        );
    }
}