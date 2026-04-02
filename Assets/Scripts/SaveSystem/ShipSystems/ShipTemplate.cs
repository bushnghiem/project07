using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Ships/Ship Template")]
public class ShipTemplate : ScriptableObject
{
    [SerializeField] private string templateID;
    public string TemplateID => templateID;

    public string displayName;

    [Header("Base Stats")]
    [SerializeField] private List<BaseStatEntry> baseStats;

    private Dictionary<ShipStatType, float> baseStatMap;
    public IReadOnlyDictionary<ShipStatType, float> BaseStatMap => baseStatMap;

    private void OnEnable()
    {
        BuildStatDictionary();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Rebuild in editor when values change
        BuildStatDictionary();
    }
#endif

    private void BuildStatDictionary()
    {
        if (baseStatMap == null)
            baseStatMap = new Dictionary<ShipStatType, float>();
        else
            baseStatMap.Clear();

        if (baseStats == null)
            return;

        foreach (var entry in baseStats)
        {
            if (baseStatMap.ContainsKey(entry.statType))
            {
                Debug.LogWarning(
                    $"Duplicate stat '{entry.statType}' on ShipTemplate '{name}'. Last value will be used.",
                    this
                );
            }

            baseStatMap[entry.statType] = entry.value;
        }
    }

    public float GetBaseStat(ShipStatType statType)
    {
        EnsureInitialized();

        return baseStatMap.TryGetValue(statType, out var value)
            ? value
            : 0f;
    }

    private void EnsureInitialized()
    {
        if (baseStatMap == null || baseStatMap.Count == 0)
        {
            BuildStatDictionary();
        }
    }

    [Header("Starting Loadout")]
    [SerializeField] private string startingActiveItemID;
    public string StartingActiveItemID => startingActiveItemID;

    [SerializeField] private List<string> startingPassiveItemIDs = new();
    public IReadOnlyList<string> StartingPassiveItemIDs => startingPassiveItemIDs;

    [SerializeField] private string startingProjectileID;
    public string StartingProjectileID => startingProjectileID;
}