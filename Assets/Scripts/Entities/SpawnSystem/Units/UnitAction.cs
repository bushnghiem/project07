using UnityEngine;

public enum ActionType
{
    Move,
    Shoot,
    Item,
    Ability
}

public class UnitAction
{
    // Who performs action
    public UnitBase actor;

    // What kind of action
    public ActionType actionType;

    // Shared targeting data
    public Vector3 direction;

    public Vector3 targetPosition;

    [Range(0f, 1f)]
    public float powerPercent;

    // Optional payloads
    public Projectile projectile;

    public ActiveItemInstance activeItem;

    public ItemTargetData itemTargetData;

    // Camera/action tracking
    public ActionContext actionContext;

    // Costs
    public int apCost = 1;
}