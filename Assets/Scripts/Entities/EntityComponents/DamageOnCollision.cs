using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField] private float contactDamage = 15f;
    [SerializeField] private float knockbackStrength = 12f;

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Hit");
        Entity entity = collision.collider.GetComponentInParent<Entity>();
        if (entity == null) return;

        entity.Hurt(contactDamage);

        Rigidbody rb = collision.rigidbody;
        if (rb != null)
        {
            Vector3 direction = (collision.transform.position - transform.position).normalized;

            rb.AddForce(direction.normalized * knockbackStrength, ForceMode.Impulse);
        }
    }

    public void SetCollisionStats(float damage, float knockback)
    {
        contactDamage = damage;
        knockbackStrength = knockback;
    }
}
