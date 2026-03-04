using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ProjectileSaveData
{
    public string projectileID;

    [Header("Stat Modifiers")]
    public List<ProjectileStatModifier> statModifiers = new List<ProjectileStatModifier>();

    [Header("Lifetime")]
    public bool useLifetimeOverride;

    [Header("Stop Detection")]
    public bool dieWhenStoppedOverride;

    [Header("Explosion")]
    public bool doesExplodeOverride;
    public ExplosionStats bonusExplosionStats;
}