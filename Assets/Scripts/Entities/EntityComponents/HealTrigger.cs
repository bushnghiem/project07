using UnityEngine;

public class HealTrigger : MonoBehaviour
{
    [SerializeField] private float healing = 10f;
    [SerializeField] private float healCooldown = 1f;

    private float lastHealTime;

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time < lastHealTime + healCooldown) return;

        Entity entity = other.GetComponentInParent<Entity>();
        if (entity == null) return;

        entity.Heal(healing);
        lastHealTime = Time.time;
    }
}
