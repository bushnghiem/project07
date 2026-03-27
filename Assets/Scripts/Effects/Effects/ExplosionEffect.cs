using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Effect/Explosion")]
public class ExplosionEffect : Effect
{
    public float radius = 5f;
    public float damage = 50f;
    public float force = 500f;
    public float upwardModifier = 2f;
    public LayerMask damageLayers;

    public GameObject visualPrefab;

    public override void Execute(EffectContext context)
    {
        Vector3 position = context.position;

        if (visualPrefab != null)
            Object.Instantiate(visualPrefab, position, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(position, radius, damageLayers);

        HashSet<Entity> hitEntities = new();

        foreach (var hit in hits)
        {
            Entity entity = hit.GetComponent<Entity>();
            if (entity != null && hitEntities.Add(entity))
            {
                float dist = Vector3.Distance(position, hit.transform.position);
                float t = Mathf.Clamp01(dist / radius);
                float finalDamage = Mathf.Lerp(damage, 0f, t);

                entity.Hurt(finalDamage);
            }

            Rigidbody rb = hit.attachedRigidbody;
            if (rb != null)
            {
                rb.AddExplosionForce(force, position, radius, upwardModifier, ForceMode.Impulse);
            }
        }
    }
}