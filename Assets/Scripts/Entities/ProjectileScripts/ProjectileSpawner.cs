using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab;

    private void OnEnable() => ProjectileSpawnEvent.OnProjectileSpawn += SpawnProjectile;
    private void OnDisable() => ProjectileSpawnEvent.OnProjectileSpawn -= SpawnProjectile;

    public void SpawnProjectile(
        Vector3 position,
        Vector3 direction,
        float force,
        Projectile stats,
        UnitBase owner)
    {
        Quaternion rot = Quaternion.LookRotation(direction);
        GameObject obj = Instantiate(projectilePrefab, position, rot);

        var instance = obj.GetComponent<ProjectileInstance>();

        instance.Initialize(stats, owner);

        instance.Fling(direction, force);

        ProjectileSpawnEvent.AddCamFollow?.Invoke(instance);
    }
}