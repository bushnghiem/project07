using UnityEngine;
using System;

public static class CameraEvent
{
    public static Action LockCamera;
    public static Action UnlockCamera;
    public static Action RecenterCamera;

    public static Action<ICameraTarget> FollowTarget;
    public static Action AttackFinished;
}
