using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ActiveItemDatabase : MonoBehaviour
{
    public static ActiveItemDatabase Instance;

    [SerializeField] private List<ActiveItem> items;

    private Dictionary<string, ActiveItem> lookup;

    private void Awake()
    {
        Instance = this;
        lookup = items.ToDictionary(i => i.itemID);
    }

    public ActiveItem Get(string id)
    {
        if (lookup.TryGetValue(id, out var item))
            return item;

        Debug.LogError("Item not found: " + id);
        return null;
    }
}