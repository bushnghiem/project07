using UnityEngine;

[CreateAssetMenu(menuName = "Ships/Visual Data")]
public class ShipVisualData : ScriptableObject
{
    [Header("Visual Prefab")]
    public GameObject visualPrefab;

    [Header("Visual Scale")]
    public float visualScale = 1f;

    [Header("UI")]
    public Sprite shipIcon;

    [Header("Optional")]
    public Material overrideMaterial;
}