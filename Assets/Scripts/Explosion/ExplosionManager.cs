using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    public Explosion explosionPrefab;

    private void OnEnable()
    {
        ExplodeEvent.OnExplode += HandleExplode;
    }

    private void OnDisable()
    {
        ExplodeEvent.OnExplode -= HandleExplode;
    }

    public void HandleExplode(ExplosionStats stats, Vector3 pos)
    {
        Explosion explosion = Instantiate(
            explosionPrefab,
            pos,
            Quaternion.identity
        );

        explosion.radius = stats.radius;
        explosion.damage = stats.damage;
        explosion.force = stats.force;
        explosion.damageLayers = stats.damageLayers;

        explosion.Explode(pos);
    }
}
