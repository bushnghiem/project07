using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class ShipRunData
{
    public string uniqueID;
    public string templateID;

    public float currentHealth;
    public bool isDead;

    public List<StatModifier> statModifiers = new();

    public ActiveItemSaveData currentActiveItem;
    public ProjectileSaveData currentProjectile;
}

