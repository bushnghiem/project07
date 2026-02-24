using UnityEngine;

public abstract class ActiveItem : ScriptableObject
{
    public string activeItemID;
    public string activeItemName;
    public Sprite icon;

    public int cooldownTurns = 2;

    public abstract void Activate(Unit user, Unit target);
}
