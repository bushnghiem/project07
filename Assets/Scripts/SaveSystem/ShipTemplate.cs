using UnityEngine;

[CreateAssetMenu(menuName = "Ship Template")]
public class ShipTemplate : ScriptableObject
{
    public string templateID;
    public string displayName;

    public int baseHealth;
    public int baseShotStrength;
    public int baseMoveStrength;
}


