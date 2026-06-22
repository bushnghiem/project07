using UnityEngine;

[CreateAssetMenu(menuName = "Projectiles/Visual Data")]
public class ProjectileVisualData : ScriptableObject
{
    public GameObject visualPrefab;
    public Material overrideMaterial;
    public Sprite icon;
    public float visualScale = 1f;
}
