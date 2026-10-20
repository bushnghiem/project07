using UnityEngine;

public enum EnemySpawnMode
{
    Preset,
    RandomAnchor,
    FullyRandom
}

[System.Serializable]
public class EnemySpawnSettings
{
    [Header("Random Anchor")]
    public Vector3 anchorAreaSize = new Vector3(20f, 0f, 20f);

    [Header("Fully Random Placement")]
    public Vector3 randomAreaSize = new Vector3(40f, 0f, 40f);

    public float minDistanceBetweenShips = 5f;
    public int maxAttemptsPerShip = 30;
}