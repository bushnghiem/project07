using UnityEngine;

public class CollisionVFXProfile : MonoBehaviour
{
    [SerializeField]
    private CollisionMaterial material = CollisionMaterial.Metal;

    public CollisionMaterial Material => material;

    public void SetMaterial(CollisionMaterial newMaterial)
    {
        material = newMaterial;
    }
}