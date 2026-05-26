using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "AI/AI Database")]
public class AIDatabase : ScriptableObject
{
    public List<EnemyAIBehavior> behaviors;

    private Dictionary<string, EnemyAIBehavior> lookup;

    private void Build()
    {
        lookup = new();

        foreach (var b in behaviors)
        {
            if (b == null)
                continue;

            lookup[b.BehaviorID] = b;
        }
    }

    public EnemyAIBehavior Get(string id)
    {
        if (lookup == null)
            Build();

        lookup.TryGetValue(id, out var result);

        return result;
    }
}