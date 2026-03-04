using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Projectiles/Projectile Database")]
public class ProjectileDatabase : ScriptableObject
{
    [SerializeField] private List<Projectile> projectiles;

    private Dictionary<string, Projectile> lookup;

    public void Initialize()
    {
        lookup = projectiles.ToDictionary(p => p.ProjectileID);
    }

    public Projectile GetProjectile(string id)
    {
        if (lookup == null)
            Initialize();

        if (lookup.TryGetValue(id, out var projectile))
            return projectile;

        Debug.LogError("Projectile not found: " + id);
        return null;
    }
}