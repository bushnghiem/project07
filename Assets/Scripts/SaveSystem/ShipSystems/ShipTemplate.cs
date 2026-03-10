using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Ships/Ship Template")]
public class ShipTemplate : ScriptableObject
{
    [SerializeField] private string templateID;
    public string TemplateID => templateID;

    public string displayName;

    [Header("Base Stats")]
    public List<BaseStatEntry> baseStats;

    [Header("Starting Loadout")]
    [Tooltip("ID of the starting active item (from ActiveItemDatabase)")]
    [SerializeField] private string startingActiveItemID;
    public string StartingActiveItemID => startingActiveItemID;

    [Tooltip("IDs of the starting passive items (from PassiveItemDatabase)")]
    public List<string> startingPassiveItemIDs = new List<string>();

    [Tooltip("ID of the starting projectile (from ProjectileDatabase)")]
    [SerializeField] private string startingProjectileID;
    public string StartingProjectileID => startingProjectileID;

    public float GetBaseStat(ShipStatType statType)
    {
        foreach (var entry in baseStats)
        {
            if (entry.statType == statType)
                return entry.value;
        }

        return 0f;
    }
}