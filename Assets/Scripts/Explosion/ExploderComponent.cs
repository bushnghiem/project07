using UnityEngine;

public class ExploderComponent : MonoBehaviour
{
    public ExplosionStats stats;

    public void SetExplosionStats(ExplosionStats newStats)
    {
        stats = newStats;
    }

    public void StartExplosion(Vector3 position)
    {
        ExplodeEvent.Trigger(stats, position);
    }
}