using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RandomEnvironmentPrefab
{
    public GameObject prefab;

    [Range(0f, 100f)]
    public float weight = 1f;

    public float collisionRadius = 2f;

    public Vector3 minScale = Vector3.one;
    public Vector3 maxScale = Vector3.one;
}

[System.Serializable]
public class RandomEnvironmentSettings
{
    public int objectCount = 10;

    public List<RandomEnvironmentPrefab> prefabs;

    public float minDistanceFromShips = 10f;
    public float minDistanceFromEnvironment = 5f;

    public float minDistanceFromCenter = 5f;
    public float maxDistanceFromCenter = 30f;

    public int maxAttemptsPerObject = 30;
}