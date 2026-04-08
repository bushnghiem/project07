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

    public float Apply(float currentValue)
    {
        float value = currentValue + flatBonus;
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

    [Header("Effects")]
    public List<Effect> effects;

    [Header("Base Stats")]
    [SerializeField] private List<ProjectileBaseStatEntry> baseStats;

    [Header("Visual")]
    public float scale = 1f;

    private Dictionary<ProjectileStatType, float> baseStatMap;
    public IReadOnlyDictionary<ProjectileStatType, float> BaseStatMap => baseStatMap;

    private void OnEnable()
    {
        BuildStatDictionary();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        BuildStatDictionary();
    }
#endif

    private void BuildStatDictionary()
    {
        if (baseStatMap == null)
            baseStatMap = new Dictionary<ProjectileStatType, float>();
        else
            baseStatMap.Clear();

        if (baseStats == null)
            return;

        foreach (var entry in baseStats)
        {
            if (baseStatMap.ContainsKey(entry.statType))
            {
                Debug.LogWarning(
                    $"Duplicate stat '{entry.statType}' on Projectile '{name}'. Last value will be used.",
                    this
                );
            }

            baseStatMap[entry.statType] = entry.value;
        }
    }

    public float GetBaseStat(ProjectileStatType statType)
    {
        EnsureInitialized();

        return baseStatMap.TryGetValue(statType, out var value)
            ? value
            : 0f;
    }

    private void EnsureInitialized()
    {
        if (baseStatMap == null || baseStatMap.Count == 0)
        {
            BuildStatDictionary();
        }
    }

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
}