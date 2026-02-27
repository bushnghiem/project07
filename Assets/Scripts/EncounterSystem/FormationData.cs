using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/Formation")]
public class FormationData : ScriptableObject
{
    public List<Vector3> positions;
}
