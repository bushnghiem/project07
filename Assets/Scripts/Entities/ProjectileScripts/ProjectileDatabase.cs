using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ProjectileDatabase : MonoBehaviour
{
    public static ProjectileDatabase Instance;
    [SerializeField] private List<Projectile> projectiles;

    private Dictionary<string, Projectile> lookup;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.Log("ProjectileDatabase exists, delete");
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        lookup = projectiles.ToDictionary(p => p.projectileID);
        Debug.Log($"ProjectileDatabase initialized with {lookup.Count} projectiles.");
    }

    public Projectile GetProjectile(string id)
    {
        if (lookup.TryGetValue(id, out var projectile))
            return projectile;

        Debug.LogError("Projectile not found: " + id);
        return null;
    }
}
