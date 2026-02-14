using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        ProjectileSpawnEvent.OnProjectileSpawn += SpawnProjectile;
    }

    private void OnDisable()
    {
        ProjectileSpawnEvent.OnProjectileSpawn -= SpawnProjectile;
    }

    public void SpawnProjectile(Vector3 position, Vector3 direction, float forceStrength)
    {
        Quaternion rotation = Quaternion.LookRotation(direction);
        GameObject projectile = Instantiate(projectilePrefab, position, rotation);
        projectile.GetComponent<Projectile>().Fling(direction, forceStrength);
    }
}
