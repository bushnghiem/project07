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
    [SerializeField] private string startingActiveItemID;
    public string StartingActiveItemID => startingActiveItemID;
    public List<string> startingPassiveItemIDs;

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


