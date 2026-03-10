using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Items/Item Database")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] private List<Item> items;

    private Dictionary<string, Item> lookup;

    private void OnEnable()
    {
        lookup = items.ToDictionary(i => i.itemID);
    }

    public Item GetItem(string id)
    {
        if (lookup.TryGetValue(id, out var item))
            return item;

        Debug.LogError("Item not found: " + id);
        return null;
    }
}