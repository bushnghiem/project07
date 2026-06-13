using UnityEngine;

[CreateAssetMenu(menuName = "Ships/Visual Data")]
public class ShipVisualData : ScriptableObject
{
    [Header("Visual Prefab")]
    public GameObject visualPrefab;

    [Header("UI")]
    public Sprite shipIcon;

    [Header("Optional")]
    public Material overrideMaterial;
}