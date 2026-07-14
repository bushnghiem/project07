using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ActionContextTracker : MonoBehaviour
{
    public static ActionContextTracker Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void TrackUntil(
        ActionContext context,
        ICameraTarget target,
        Func<bool> finished)
    {
        StartCoroutine(TrackRoutine(context, target, finished));
    }

    public void TrackProjectile(
        ActionContext context,
        ProjectileInstance projectile)
    {
        TrackUntil(
            context,
            projectile,
            () =>
                projectile.isDead ||
                projectile.HasStopped);
    }

    public void TrackWhileMoving(
    ActionContext context,
    Rigidbody rb,
    Entity entity)
    {
        StartCoroutine(TrackWhileMovingRoutine(context, rb, entity));
    }

    private IEnumerator TrackWhileMovingRoutine(
        ActionContext context,
        Rigidbody rb,
        Entity entity)
    {

        if (!context.ContainsTarget(entity))
        {
            context.AddTarget(entity);
        }

        // Wait for physics to apply forces
        yield return new WaitForFixedUpdate();

        float stoppedTimer = 0f;

        while (!entity.isDead && rb != null)
        {
            if (rb.linearVelocity.sqrMagnitude < 0.05f)
            {
                stoppedTimer += Time.deltaTime;

                if (stoppedTimer >= 0.25f)
                    break;
            }
            else
            {
                stoppedTimer = 0f;
            }

            yield return null;
        }

        context.RemoveTarget(entity);
    }

    public void TrackForSeconds(
        ActionContext context,
        Entity entity,
        float seconds)
    {
        StartCoroutine(TrackForSecondsRoutine(context, entity, seconds));
    }

    private IEnumerator TrackForSecondsRoutine(
        ActionContext context,
        Entity entity,
        float seconds)
    {
        if (!context.ContainsTarget(entity))
        {
            context.AddTarget(entity);
        }

        float elapsed = 0f;

        while (elapsed < seconds && !entity.isDead)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        context.RemoveTarget(entity);
    }

    private IEnumerator TrackRoutine(
        ActionContext context,
        ICameraTarget target,
        Func<bool> finished)
    {
        if (!context.ContainsTarget(target))
        {
            context.AddTarget(target);
        }

        yield return new WaitUntil(finished);

        if (context.GetTargets().Contains(target))
        {
            context.RemoveTarget(target);
        }
    }
}
