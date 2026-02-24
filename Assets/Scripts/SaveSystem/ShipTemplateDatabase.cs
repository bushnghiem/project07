using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ShipTemplateDatabase : MonoBehaviour
{
    public static ShipTemplateDatabase Instance;

    [SerializeField] private List<ShipTemplate> templates;

    private Dictionary<string, ShipTemplate> lookup;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.Log("ShipTemplateDatabase exists, delete");
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        lookup = templates.ToDictionary(t => t.templateID);

        Debug.Log($"ShipTemplateDatabase initialized with {lookup.Count} templates.");
    }

    public ShipTemplate GetTemplate(string id)
    {
        if (lookup.TryGetValue(id, out var template))
            return template;

        Debug.LogError($"ShipTemplate not found: {id}");
        return null;
    }
}