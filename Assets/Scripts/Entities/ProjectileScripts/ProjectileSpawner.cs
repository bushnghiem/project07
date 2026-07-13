using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab;

    private void OnEnable() => ProjectileSpawnEvent.OnProjectileSpawn += SpawnProjectile;
    private void OnDisable() => ProjectileSpawnEvent.OnProjectileSpawn -= SpawnProjectile;

    public void SpawnProjectile(ProjectileSpawnRequest request)
    {
        Quaternion rot =
            Quaternion.LookRotation(request.Direction);

        GameObject obj =
            Instantiate(
                projectilePrefab,
                request.Position,
                rot);

        var instance =
            obj.GetComponent<ProjectileInstance>();

        instance.Initialize(
            request.Projectile,
            request.Owner,
            request.ActionContext);

        instance.Fling(
            request.Direction,
            request.Force);
    }
}