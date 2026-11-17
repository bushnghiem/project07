using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "CollisionVFXLibrary",
    menuName = "Combat/Collision VFX Library"
)]
public class CollisionVFXLibrary : ScriptableObject
{
    [Serializable]
    public class CollisionEffect
    {
        public CollisionMaterial materialA;
        public CollisionMaterial materialB;

        public GameObject effect;

        [Tooltip("Minimum relative velocity required to play this effect.")]
        public float minimumImpactSpeed = 1f;
    }

    public List<CollisionEffect> effects = new();

    public GameObject GetEffect(
        CollisionMaterial a,
        CollisionMaterial b,
        float impactSpeed)
    {
        foreach (var entry in effects)
        {
            bool matches =
                (entry.materialA == a && entry.materialB == b) ||
                (entry.materialA == b && entry.materialB == a);

            if (matches &&
                impactSpeed >= entry.minimumImpactSpeed)
            {
                return entry.effect;
            }
        }

        return null;
    }
}
