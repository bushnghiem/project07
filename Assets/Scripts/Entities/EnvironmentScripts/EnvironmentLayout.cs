using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/Environment Layout")]
public class EnvironmentLayout : ScriptableObject
{
    public EnvironmentSpawnMode spawnMode;

    // Used for Preset
    public List<EnvironmentData> environmentObjects;

    // Used for Random
    public RandomEnvironmentSettings randomSettings;
}
