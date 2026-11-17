using UnityEngine;

public class CollisionVFX : MonoBehaviour
{
    [SerializeField] private CollisionVFXLibrary vfxLibrary;
    [SerializeField] private float minimumImpactSpeed = 1f;
    [SerializeField] private float maximumImpactSpeed = 20f;

    [Header("Particle Scaling")]
    [SerializeField] private float minimumParticleSize = 0.5f;
    [SerializeField] private float maximumParticleSize = 2f;

    private CollisionVFXProfile ownProfile;

    private void Awake()
    {
        ownProfile = GetComponentInParent<CollisionVFXProfile>();
    }

    public void PlayCollisionEffect(Collision collision)
    {
        if (ownProfile == null || vfxLibrary == null)
            return;

        CollisionVFXProfile otherProfile =
            collision.collider.GetComponentInParent<CollisionVFXProfile>();

        if (otherProfile == null)
            return;

        // Only allow one of the two entities to spawn the VFX.
        if (ownProfile.GetInstanceID() > otherProfile.GetInstanceID())
            return;

        float impactSpeed = collision.relativeVelocity.magnitude;

        if (impactSpeed < minimumImpactSpeed)
            return;

        GameObject effect = vfxLibrary.GetEffect(
            ownProfile.Material,
            otherProfile.Material,
            impactSpeed
        );

        if (effect == null)
            return;

        ContactPoint contact = collision.GetContact(0);

        // Orient particles away from the collision surface.
        Quaternion rotation = Quaternion.LookRotation(
            contact.normal
        );

        float intensity = Mathf.InverseLerp(
            minimumImpactSpeed,
            maximumImpactSpeed,
            impactSpeed
        );

        float size = Mathf.Lerp(
            minimumParticleSize,
            maximumParticleSize,
            intensity
        );

        GameObject instance = Instantiate(
            effect,
            contact.point,
            rotation
        );

        instance.transform.localScale *= size;
    }
}