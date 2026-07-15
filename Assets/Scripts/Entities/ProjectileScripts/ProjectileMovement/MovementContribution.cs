using UnityEngine;

public enum MovementReference
{
    Launch,
    Velocity
}

public struct MovementContribution
{
    public MovementReference Reference;

    public float ForwardAcceleration;

    public float LateralAcceleration;

    // Optional acceleration in world space.
    // Used for things like homing, gravity wells, magnets, etc.
    public Vector3 WorldAcceleration;

    public static MovementContribution Zero =>
        new MovementContribution
        {
            Reference = MovementReference.Velocity
        };
}