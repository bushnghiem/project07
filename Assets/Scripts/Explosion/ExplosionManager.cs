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

    private void HandleExplode(ExplosionStats stats, Vector3 pos)
    {
        Explosion explosion = Instantiate(explosionPrefab, pos, Quaternion.identity);
        explosion.Initialize(stats);
    }
}