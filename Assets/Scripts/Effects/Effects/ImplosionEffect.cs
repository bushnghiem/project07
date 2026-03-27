using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Implosion")]
public class ImplosionEffect : Effect
{
    public float radius = 5f;
    public float pullForce = 300f;
    public LayerMask affectedLayers;

    public GameObject visualPrefab;

    public override void Execute(EffectContext context)
    {
        Vector3 position = context.position;

        if (visualPrefab != null)
            Object.Instantiate(visualPrefab, position, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(position, radius, affectedLayers);

        foreach (var hit in hits)
        {
            Rigidbody rb = hit.attachedRigidbody;
            if (rb != null)
            {
                Vector3 dir = (position - rb.position).normalized;
                rb.AddForce(dir * pullForce, ForceMode.Impulse);
            }
        }
    }
}