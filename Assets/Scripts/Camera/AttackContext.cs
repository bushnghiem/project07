using UnityEngine;
using System.Collections.Generic;

public class AttackContext
{
    private CameraTargetGroup cameraGroup = new();

    private HashSet<ProjectileInstance> projectiles = new();

    public ICameraTarget CameraTarget => cameraGroup;

    private bool finished;


    public void RegisterProjectile(ProjectileInstance projectile)
    {
        projectiles.Add(projectile);
        cameraGroup.Add(projectile);
    }


    public void UnregisterProjectile(ProjectileInstance projectile)
    {
        projectiles.Remove(projectile);
        cameraGroup.Remove(projectile);

        CheckFinished();
    }


    public void ProjectileStopped(ProjectileInstance projectile)
    {
        cameraGroup.Remove(projectile);

        CheckFinished();
    }


    private void CheckFinished()
    {
        if (finished)
            return;

        if (cameraGroup.IsEmpty)
        {
            finished = true;
            CameraEvent.AttackFinished?.Invoke();
        }
    }
}