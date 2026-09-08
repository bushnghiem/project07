using UnityEngine;

[System.Serializable]
public struct ProjectileSpawnData
{
    public Vector3 direction;
    public float force;

    // Offset relative to the shooter's normal projectile spawn position.
    public Vector3 spawnOffset;

    public Projectile projectile;
}
