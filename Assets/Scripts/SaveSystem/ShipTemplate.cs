using UnityEngine;

[CreateAssetMenu(fileName = "Ship Template", menuName = "Scriptable Objects/Ship Template")]
public class ShipTemplate : ScriptableObject
{
    public string templateID;
    public string displayName;

    public float baseHealth;
    public int baseStartingShield;
    public float baseShotStrength;
    public float baseMoveStrength;
    public float baseMass;
    public int baseInitiative;

    public float baseCollisionDamage;
    public float baseCollisionKnockback;
}


