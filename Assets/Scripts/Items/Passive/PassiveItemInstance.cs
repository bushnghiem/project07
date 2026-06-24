using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PassiveItemInstance
{
    public PassiveItem itemData;

    // Runtime objects added by this item
    public List<Object> grantedObjects = new();

    public PassiveItemInstance(PassiveItem data)
    {
        itemData = data;
    }

    public void Apply(Unit unit)
    {
        itemData.ApplyEffect(unit, this);
    }

    public void Remove(Unit unit)
    {
        itemData.RemoveEffect(unit, this);
    }
}