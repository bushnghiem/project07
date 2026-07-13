using System.Collections.Generic;
using UnityEngine;

public class CameraTargetGroup : ICameraTarget
{
    private readonly List<ICameraTarget> targets = new();

    public Vector3 Position
    {
        get
        {
            if (targets.Count == 0)
                return Vector3.zero;

            Vector3 average = Vector3.zero;

            foreach (var target in targets)
            {
                average += target.Position;
            }

            return average / targets.Count;
        }
    }

    public bool IsEmpty => targets.Count == 0;

    public void Add(ICameraTarget target)
    {
        if (!targets.Contains(target))
            targets.Add(target);
    }

    public void Remove(ICameraTarget target)
    {
        targets.Remove(target);
    }
}