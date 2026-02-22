using UnityEngine;

[System.Serializable]
public class ShipRunData
{
    public string uniqueID;
    public string templateID;   // <-- Add this

    public float currentHealth;
    public bool isDead;

    public float bonusMaxHealth;
    public float bonusShotStrength;
    public float bonusMoveStrength;
    public float bonusMass;
    public int bonusInitiative;

    public ActiveItem currentItem;

    public float GetMaxHealth(ShipTemplate template)
    {
        return template.baseHealth + bonusMaxHealth;
    }

    public float GetShotStrength(ShipTemplate template)
    {
        return template.baseShotStrength + bonusShotStrength;
    }

    public float GetMoveStrength(ShipTemplate template)
    {
        return template.baseMoveStrength + bonusMoveStrength;
    }

    public float GetMass(ShipTemplate template)
    {
        return template.baseMass + bonusMass;
    }

    public int GetInitiative(ShipTemplate template)
    {
        return template.baseInitiative + bonusInitiative;
    }
}

