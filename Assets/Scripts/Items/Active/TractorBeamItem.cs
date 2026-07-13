using UnityEngine;

[CreateAssetMenu(menuName = "Items/Active/Tractor Beam")]
public class TractorBeamItem : ActiveItem
{
    public float force = 20f;

    public override ItemTargetType TargetType => ItemTargetType.Direction;

    public override void Execute(
        Unit user,
        ItemTargetData data,
        ActionContext context
    )
    {
        Vector3 origin = user.Position;
        Vector3 dir = data.direction;
        dir.y = 0f;
        dir.Normalize();

        Collider[] hits = Physics.OverlapSphere(origin, effectRadius);

        foreach (var hit in hits)
        {
            Rigidbody rb = hit.attachedRigidbody;
            if (rb == null || rb.isKinematic)
                continue;

            Vector3 toTarget = rb.position - origin;
            toTarget.y = 0f;

            float dist = toTarget.magnitude;
            if (dist < 0.01f)
                continue;

            Vector3 toDir = toTarget / dist;

            float dot = Vector3.Dot(dir, toDir);
            float cosThreshold = Mathf.Cos(coneAngle * 0.5f * Mathf.Deg2Rad);

            if (dot < cosThreshold)
                continue;

            Vector3 pullDir = (origin - rb.position).normalized;

            rb.AddForce(pullDir * force, ForceMode.Impulse);
        }
    }
}