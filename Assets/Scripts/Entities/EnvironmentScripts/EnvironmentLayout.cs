using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/Environment Layout")]
public class EnvironmentLayout : ScriptableObject
{
    public List<EnvironmentData> environmentObjects;
}
