using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float damageCooldown = 1f;

    private float lastDamageTime;

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time < lastDamageTime + damageCooldown) return;

        Entity entity = other.GetComponentInParent<Entity>();
        if (entity == null) return;

        entity.Hurt(damage);
        lastDamageTime = Time.time;
    }
}
