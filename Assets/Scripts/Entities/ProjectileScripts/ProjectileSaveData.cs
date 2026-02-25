using UnityEngine;

[System.Serializable]
public class ProjectileSaveData
{
    public string projectileID;

    [Header("Combat")]
    public float bonusMaxHealth = 0f;
    public float bonusCollisionDamage = 0f;
    public int bonusStartingShield = 0;

    [Header("Physics")]
    public float bonusMass = 0f;
    public float bonusCollisionKnockback = 0f;

    [Header("Lifetime")]
    public bool useLifetimeOverride;

    [Header("Stop Detection")]
    public bool dieWhenStoppedOverride;

    [Header("Explosion")]
    public bool doesExplodeOverride;
    public ExplosionStats bonusExplosionStats;
}
