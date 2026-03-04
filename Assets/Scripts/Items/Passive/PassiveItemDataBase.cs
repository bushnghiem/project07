using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Items/Passive/Passive Item Database")]
public class PassiveItemDatabase : ScriptableObject
{
    [SerializeField] private List<PassiveItem> passiveItems;

    private Dictionary<string, PassiveItem> lookup;

    private void OnEnable()
    {
        lookup = passiveItems.ToDictionary(p => p.passiveItemID);
    }

    public PassiveItem GetPassiveItem(string id)
    {
        if (lookup.TryGetValue(id, out var item))
            return item;

        Debug.LogError($"Passive Item not found: {id}");
        return null;
    }
}
