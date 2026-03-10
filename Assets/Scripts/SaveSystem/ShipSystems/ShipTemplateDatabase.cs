using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Ships/Ship Template Database")]
public class ShipTemplateDatabase : ScriptableObject
{
    [SerializeField] private List<ShipTemplate> templates;

    private Dictionary<string, ShipTemplate> lookup;

    public void Initialize()
    {
        lookup = templates.ToDictionary(t => t.TemplateID);
    }

    public ShipTemplate GetTemplate(string id)
    {
        if (lookup == null)
            Initialize();

        if (lookup.TryGetValue(id, out var template))
            return template;

        Debug.LogError($"ShipTemplate not found: {id}");
        return null;
    }

    public List<ShipTemplate> GetAllTemplates() => templates;
}