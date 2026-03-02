using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/Encounter Pool")]
public class EncounterPool : ScriptableObject
{
    public List<EncounterData> encounters;
}
