using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionContext
{
    private CameraTargetGroup cameraGroup = new();

    private HashSet<ICameraTarget> targets = new();

    public ICameraTarget CameraTarget => cameraGroup;


    public event Action<ICameraTarget> OnTargetChanged;
    public event Action OnContextFinished;


    public void AddTarget(ICameraTarget target)
    {
        if (target == null)
            return;

        targets.Add(target);

        cameraGroup.Add(target);

        OnTargetChanged?.Invoke(CameraTarget);
    }


    public void RemoveTarget(ICameraTarget target)
    {
        if (target == null)
            return;

        targets.Remove(target);

        cameraGroup.Remove(target);


        if (targets.Count == 0)
        {
            OnContextFinished?.Invoke();
        }
    }


    public List<ICameraTarget> GetTargets()
    {
        return new List<ICameraTarget>(targets);
    }
}