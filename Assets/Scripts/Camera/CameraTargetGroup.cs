using UnityEngine;
using System;
using System.Collections.Generic;

public class CameraTargetGroup : ICameraTarget
{
    private readonly List<ICameraTarget> targets = new();

    public Vector3 Position
    {
        get
        {
            if (targets.Count == 0)
                return Vector3.zero;

            Vector3 center = Vector3.zero;

            foreach (var target in targets)
                center += target.Position;

            return center / targets.Count;
        }
    }

    public bool IsEmpty => targets.Count == 0;

    public void Add(ICameraTarget target)
    {
        targets.Add(target);
    }

    public void Remove(ICameraTarget target)
    {
        targets.Remove(target);
    }
}
