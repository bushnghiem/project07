using UnityEngine;
using System;
using System.Collections.Generic;

public enum ProjectileStatType
{
    MaxHealth,
    StartingShield,
    CollisionDamage,
    CollisionKnockback,
    Mass
}

[Serializable]
public class ProjectileBaseStatEntry
{
    public ProjectileStatType statType;
    public float value;
}

[Serializable]
public class ProjectileStatModifier
{
    public ProjectileStatType statType;
    public float flatBonus;
    public float percentBonus;

    public float Apply(float baseValue)
    {
        float value = baseValue + flatBonus;
        value += value * percentBonus;
        return value;
    }
}

[CreateAssetMenu(fileName = "Projectile", menuName = "Projectiles/Projectile")]
public class Projectile : ScriptableObject
{
    [SerializeField] private string projectileID;
    public string ProjectileID => projectileID;

    public string projectileName;

    [Header("Base Stats")]
    public List<ProjectileBaseStatEntry> baseStats;

    [Header("Physics")]
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

    // Return base stat value
    public float GetBaseStat(ProjectileStatType statType)
    {
        foreach (var entry in baseStats)
        {
            if (entry.statType == statType)
                return entry.value;
        }
        return 0f;
    }

    // Apply saved modifiers from run data
    public void Initialize(ProjectileSaveData saveData)
    {
        foreach (var modifier in saveData.statModifiers)
        {
            for (int i = 0; i < baseStats.Count; i++)
            {
                if (baseStats[i].statType == modifier.statType)
                {
                    baseStats[i].value = modifier.Apply(baseStats[i].value);
                    break;
                }
            }
        }

        useLifetime = saveData.useLifetimeOverride;
        dieWhenStopped = saveData.dieWhenStoppedOverride;
        doesExplode = saveData.doesExplodeOverride;

        explosionStats.radius += saveData.bonusExplosionStats.radius;
        explosionStats.damage += saveData.bonusExplosionStats.damage;
        explosionStats.force += saveData.bonusExplosionStats.force;
        explosionStats.damageLayers = saveData.bonusExplosionStats.damageLayers;
    }
}