using UnityEngine;

[CreateAssetMenu(menuName = "Items/Active/Repulsor Beam")]
public class RepulsorBeamItem : ActiveItem
{
    public float force = 20f;

    public override ItemTargetType TargetType => ItemTargetType.Direction;

    public override void Execute(Unit user, ItemTargetData data)
    {
        Vector3 origin = user.Position;
        Vector3 dir = data.direction;
        dir.y = 0f;
        dir.Normalize();

        Collider[] hits = Physics.OverlapSphere(origin, effectRadius);

        float cosThreshold = Mathf.Cos(coneAngle * 0.5f * Mathf.Deg2Rad);

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

            if (dot < cosThreshold)
                continue;

            Vector3 pushDir = (rb.position - origin).normalized;

            rb.AddForce(pushDir * force, ForceMode.Impulse);
        }
    }
}