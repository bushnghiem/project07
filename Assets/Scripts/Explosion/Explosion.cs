using UnityEngine;
using System.Collections.Generic;

public class Explosion : MonoBehaviour
{
    [Header("Runtime Stats")]
    private float radius;
    private float damage;
    private float force;
    private float upwardModifier;
    private LayerMask damageLayers;

    [Header("Visuals")]
    public float baseVisualRadius = 1f;

    private bool hasExploded = false;

    public void Initialize(ExplosionStats stats)
    {
        radius = stats.radius;
        damage = stats.damage;
        force = stats.force;
        upwardModifier = stats.upwardModifier;
        damageLayers = stats.damageLayers;

        ScaleVisual();
        PlayEffects();

        Invoke(nameof(Explode), 0.02f);
    }

    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        Collider[] hits = Physics.OverlapSphere(transform.position, radius, damageLayers);

        HashSet<Entity> damagedEntities = new();

        foreach (Collider hit in hits)
        {
            Entity entity = hit.GetComponent<Entity>();
            if (entity != null && damagedEntities.Add(entity))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                float t = Mathf.Clamp01(distance / radius);

                float finalDamage = Mathf.Lerp(damage, 0f, t);
                entity.Hurt(finalDamage);
            }

            Rigidbody rb = hit.attachedRigidbody;
            if (rb != null)
            {
                rb.AddExplosionForce(
                    force,
                    transform.position,
                    radius,
                    upwardModifier,
                    ForceMode.Impulse
                );
            }
        }

        Destroy(gameObject, 2f);
    }

    private void ScaleVisual()
    {
        float multiplier = radius / baseVisualRadius;
        transform.localScale = Vector3.one * multiplier;
    }

    private void PlayEffects()
    {
        var ps = GetComponent<ParticleSystem>();
        if (ps != null) ps.Play();

        var audio = GetComponent<AudioSource>();
        if (audio != null) audio.Play();
    }
}