using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField] private float contactDamage = 15f;

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Hit");
        HealthComponent health = collision.collider.GetComponentInParent<HealthComponent>();
        if (health == null) return;

        health.Hurt(contactDamage);
    }
}
