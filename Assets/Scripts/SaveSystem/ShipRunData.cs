using UnityEngine;

[System.Serializable]
public class ShipRunData
{
    public string uniqueID;
    public string templateID;

    public float currentHealth;
    public bool isDead;

    public float bonusMaxHealth;
    public float bonusShotStrength;
    public float bonusMoveStrength;
    public float bonusMass;
    public int bonusInitiative;

    public float bonusCollisionDamage;
    public float bonusCollisionKnockback;

    public ActiveItemSaveData currentActiveItem;

    public ProjectileSaveData currentProjectile;

    public void SetDefaults()
    {
        uniqueID = "0";
        templateID = "0";
        currentHealth = 100;
        isDead = false;
        bonusMaxHealth = 0;
        bonusShotStrength = 0;
        bonusMoveStrength = 0;
        bonusMass = 0;
        bonusInitiative = 0;
        bonusCollisionDamage = 0;
        bonusCollisionKnockback = 0;
    }

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

    public float GetCollisionDamage(ShipTemplate template)
    {
        return template.baseCollisionDamage + bonusCollisionDamage;
    }

    public float GetCollisionKnockback(ShipTemplate template)
    {
        return template.baseCollisionKnockback + bonusCollisionKnockback;
    }
}

