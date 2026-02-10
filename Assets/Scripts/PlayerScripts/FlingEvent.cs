using System;
using UnityEngine;

public static class FlingEvent
{
    public static Action<float> OnPowerChanged;   // 0–1
    public static Action<Vector3, float> OnFling; // direction, force
}

