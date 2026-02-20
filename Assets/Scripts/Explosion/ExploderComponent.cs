using UnityEngine;

public class ExploderComponent : MonoBehaviour
{
    public ExplosionStats stats;

    public void StartExplosion(Vector3 explosionPosition)
    {
        ExplodeEvent.OnExplode(stats, explosionPosition);
    }
}
