using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Effect/Explosion")]
public class ExplosionEffect : Effect
{
    public List<AppliedStatusEffect> statusEffects = new ();
    public float radius = 5f;
    public float damage = 50f;
    public float force = 500f;
    public float upwardModifier = 2f;
    public LayerMask damageLayers;

    public GameObject visualPrefab;

    public override void Execute(EffectContext context)
    {
        //Debug.Log($"EXPLOSION ID: {GetInstanceID()} on frame {Time.frameCount}");
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
                Debug.Log($"Boom Hit {entity} for {damage}");

                Rigidbody rigidBody = hit.attachedRigidbody;

                if (context.actionContext != null)
                {

                    if (rigidBody != null)
                    {
                        ActionContextTracker.Instance.TrackWhileMoving(
                            context.actionContext,
                            rigidBody,
                            entity);
                    }
                    else
                    {
                        ActionContextTracker.Instance.TrackForSeconds(
                            context.actionContext,
                            entity,
                            0.5f);
                    }
                }

                entity.Hurt(DamagePresets.Explosion(damage));

                var unit = hit.GetComponent<Unit>();
                if (unit != null)
                {
                    var statusController = hit.GetComponent<StatusEffectController>();
                    if (statusController != null)
                    {
                        foreach (var applied in statusEffects)
                        {
                            statusController.ApplyEffect(applied.effect, applied.stacks);
                        }
                    }
                }
            }

            Rigidbody rb = hit.attachedRigidbody;
            if (rb != null)
            {
                rb.AddExplosionForce(force, position, radius, upwardModifier, ForceMode.Impulse);
            }
        }
    }
}