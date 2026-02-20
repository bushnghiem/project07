using UnityEngine;

[System.Serializable]
public struct ExplosionStats
{
    public float radius;
    public float damage;
    public float force;
    public LayerMask damageLayers;
}
