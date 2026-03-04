using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField] private float contactDamage = 15f;
    [SerializeField] private float knockbackStrength = 12f;

    private void OnCollisionEnter(Collision collision)
    {
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
    }

    public void SetCollisionStats(float damage, float knockback)
    {
        contactDamage = damage;
        knockbackStrength = knockback;
    }
}
