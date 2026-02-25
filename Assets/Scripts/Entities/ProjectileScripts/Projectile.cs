using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Scriptable Objects/Projectile")]
public class Projectile : ScriptableObject
{
    [Header("Save ID (must be unique)")]
    public string projectileID;

    [Header("Identity")]
    public string projectileName;

    [Header("Combat")]
    public float maxHealth = 1f;
    public float collisionDamage = 10f;
    public int startingShield = 0;

    [Header("Physics")]
    public float mass = 1f;
    public float collisionKnockback = 20f;
    public float linearDamping = 2f;
    public float angularDamping = 2f;

    [Header("Lifetime")]
    public bool useLifetime = true;
    public float lifeTime = 5f;

    [Header("Stop Detection")]
    public bool dieWhenStopped = true;
    public float velocityThreshold = 0.1f;
    public float stopTimeRequired = 0.5f;

    [Header("Explosion")]
    public bool doesExplode = true;
    public ExplosionStats explosionStats;

    public void Initialize(ProjectileSaveData saveData)
    {
        maxHealth += saveData.bonusMaxHealth;
        collisionDamage += saveData.bonusCollisionDamage;
        startingShield += saveData.bonusStartingShield;
        mass += saveData.bonusMass;
        collisionKnockback += saveData.bonusCollisionKnockback;
        useLifetime = saveData.useLifetimeOverride;
        dieWhenStopped = saveData.dieWhenStoppedOverride;
        doesExplode = saveData.doesExplodeOverride;
        explosionStats.radius += saveData.bonusExplosionStats.radius;
        explosionStats.damage += saveData.bonusExplosionStats.damage;
        explosionStats.force += saveData.bonusExplosionStats.force;
        explosionStats.damageLayers = saveData.bonusExplosionStats.damageLayers;
    }
}
