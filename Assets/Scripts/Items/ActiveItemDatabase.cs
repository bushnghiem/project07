using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ActiveItemDatabase : MonoBehaviour
{
    public static ActiveItemDatabase Instance;

    [SerializeField] private List<ActiveItem> activeItems;

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

        lookup = activeItems.ToDictionary(i => i.activeItemID);
        Debug.Log($"ActiveItemDatabase initialized with {lookup.Count} items.");
    }

    public ActiveItem GetActiveItem(string id)
    {
        if (lookup.TryGetValue(id, out var activeItem))
            return activeItem;

        Debug.LogError("Active Item not found: " + id);
        return null;
    }
}