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
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.Log("ActiveItemDatabase exists, delete");
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        lookup = items.ToDictionary(i => i.itemID);
        Debug.Log($"ActiveItemDatabase initialized with {lookup.Count} items.");
    }

    public ActiveItem GetItem(string id)
    {
        if (lookup.TryGetValue(id, out var item))
            return item;

        Debug.LogError("Item not found: " + id);
        return null;
    }
}