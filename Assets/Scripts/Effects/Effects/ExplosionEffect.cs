using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Effect/Explosion")]
public class ExplosionEffect : Effect
{
    public SoundDefinition explosionSound;
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

        AudioManager.Play(explosionSound, position);

        CameraEvent.ShakeCamera?.Invoke(0.5f);

        if (visualPrefab != null)
            Object.Instantiate(visualPrefab, position, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(position, radius, damageLayers);

        HashSet<Entity> hitEntities = new();

        foreach (var hit in hits)
        {
            Entity entity = hit.GetComponentInParent<Entity>();
            if (entity != null && hitEntities.Add(entity))
            {
                //Debug.Log($"Boom Hit {entity} for {damage}");

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

                entity.Hurt(
                    DamagePresets.Explosion(
                        damage,
                        context.owner,
                        context.sourceEntity
                    )
                );

                var unit = hit.GetComponentInParent<UnitBase>();
                if (unit != null)
                {
                    var statusController =
                        unit != null
                            ? unit.GetComponent<StatusEffectController>()
                            : null;
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

            if (rb != null && !rb.isKinematic)
            {
                Vector3 direction = rb.worldCenterOfMass - position;

                direction.y = 0f;

                float distance = direction.magnitude;

                if (distance > 0.001f && distance < radius)
                {
                    direction.Normalize();

                    float falloff = 1f - (distance / radius);

                    Vector3 explosionForce =
                        direction * force * falloff;

                    // Apply force away from explosion,
                    // allowing rotational torque.
                    rb.AddForceAtPosition(
                        explosionForce,
                        hit.ClosestPoint(position),
                        ForceMode.Impulse
                    );
                }
            }
        }
    }
}