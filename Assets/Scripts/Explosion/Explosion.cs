using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float radius;
    public float damage;
    public float force;
    public float upwardModifier = 2f;
    public LayerMask damageLayers;
    public float baseVisualRadius = 1f;

    public void Explode(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(position, radius, damageLayers);

        ScaleVisual();

        foreach (Collider hit in hits)
        {
            // Damage every entity
            Entity entity = hit.GetComponent<Entity>();
            if (entity != null)
                entity.Hurt(damage);

            // Apply force to rigidbodies
            Rigidbody rb = hit.attachedRigidbody;
            if (rb != null)
            {
                rb.AddExplosionForce(
                    force,
                    position,
                    radius,
                    upwardModifier,
                    ForceMode.Impulse
                );
            }
        }
    }

    public void ScaleVisual()
    {
        float multiplier = radius / baseVisualRadius;
        transform.localScale = Vector3.one * multiplier;
    }
}
