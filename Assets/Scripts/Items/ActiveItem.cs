using UnityEngine;

[CreateAssetMenu(fileName = "ActiveItem", menuName = "Scriptable Objects/ActiveItem")]
public abstract class ActiveItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    public int cooldownTurns = 2;

    public abstract void Activate(Unit user, Unit target);
}
