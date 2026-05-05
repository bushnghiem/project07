using UnityEngine;
using System.Collections.Generic;

public class DamageOnCollision : MonoBehaviour
{
    public List<StatusEffectData> statusEffects = new();
    [SerializeField] private float contactDamage = 15f;
    [SerializeField] private float knockbackStrength = 12f;
    private float spawnTime;

    void Awake()
    {
        spawnTime = Time.time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Time.time - spawnTime < 0.05f)
            return;

        Entity entity = collision.collider.GetComponentInParent<Entity>();
        if (entity == null) return;

        entity.Hurt(contactDamage);

        Rigidbody rb = collision.rigidbody;
        if (rb != null)
        {
            Vector3 physicsImpulse = collision.impulse;

            // Additional knockback in hit direction
            Vector3 bonusDirection = (collision.transform.position - transform.position).normalized;
            Vector3 bonusImpulse = bonusDirection * knockbackStrength;

            rb.AddForce(physicsImpulse + bonusImpulse, ForceMode.Impulse);
        }
        var unit = collision.collider.GetComponent<Unit>();
        if (unit != null)
        {
            var statusController = collision.collider.GetComponent<StatusEffectController>();
            if (statusController != null)
            {
                foreach (var effectData in statusEffects)
                {
                    statusController.ApplyEffect(effectData, 10);
                }
            }
        }
    }

    public void SetCollisionStats(float damage, float knockback)
    {
        contactDamage = damage;
        knockbackStrength = knockback;
    }
}
