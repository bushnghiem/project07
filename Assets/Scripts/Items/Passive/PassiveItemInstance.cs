using UnityEngine;

[System.Serializable]
public class PassiveItemInstance
{
    public PassiveItem itemData;

    public PassiveItemInstance(PassiveItem data)
    {
        itemData = data;
    }

    public void Apply(Unit unit)
    {
        itemData.ApplyEffect(unit);
    }

    public void Remove(Unit unit)
    {
        itemData.RemoveEffect(unit);
    }
}
