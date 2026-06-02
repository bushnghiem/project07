using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PassiveItemInstance
{
    public PassiveItem itemData;

    public List<Effect> injectedEffects = new();

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
