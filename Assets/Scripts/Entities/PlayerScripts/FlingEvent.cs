using System;
using UnityEngine;

public static class FlingEvent
{
    public static event Action<float> OnPowerChanged;
    public static event Action<ActionType> OnFlingTargetingStarted;
    public static event Action OnFlingTargetingEnded;

    public static void PowerChanged(float power)
    {
        OnPowerChanged?.Invoke(power);
    }

    public static void FlingTargetingStarted(ActionType actionType)
    {
        Debug.Log("FlingTargetingStarted event fired: " + actionType);
        OnFlingTargetingStarted?.Invoke(actionType);
    }

    public static void FlingTargetingEnded()
    {
        Debug.Log("FlingTargetingEnded event fired");
        OnFlingTargetingEnded?.Invoke();
    }
}
