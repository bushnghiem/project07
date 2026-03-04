using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Items/Active/Active Item Database")]
public class ActiveItemDatabase : ScriptableObject
{
    [SerializeField] private List<ActiveItem> activeItems;

    private Dictionary<string, ActiveItem> lookup;

    private void OnEnable()
    {
        lookup = activeItems.ToDictionary(i => i.activeItemID);
    }

    public ActiveItem GetActiveItem(string id)
    {
        if (lookup.TryGetValue(id, out var activeItem))
            return activeItem;

        Debug.LogError("Active Item not found: " + id);
        return null;
    }
}