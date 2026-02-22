using UnityEngine;
using System.Collections.Generic;

public class TemplateDatabase : MonoBehaviour
{
    public static TemplateDatabase Instance;

    public List<ShipTemplate> templates;

    private Dictionary<string, ShipTemplate> lookup;

    private void Awake()
    {
        Instance = this;

        lookup = new Dictionary<string, ShipTemplate>();
        foreach (var template in templates)
        {
            lookup.Add(template.templateID, template);
        }
    }

    public ShipTemplate GetTemplate(string id)
    {
        return lookup[id];
    }
}
