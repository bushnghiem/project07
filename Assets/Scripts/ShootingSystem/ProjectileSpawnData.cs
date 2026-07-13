using UnityEngine;

[System.Serializable]
public struct ProjectileSpawnData
{
    public Vector3 position;
    public Vector3 direction;
    public float force;

    public Projectile projectile;

    public AttackContext attack;
}
