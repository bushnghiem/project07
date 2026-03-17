using UnityEngine;
using System.Linq;

public static class EnemyAIUtility
{
    // Get closest player
    public static Player GetClosestPlayer(Enemy enemy, BattleManager battleManager)
    {
        if (battleManager == null || battleManager.allPlayers.Count == 0)
            return null;

        return battleManager.allPlayers
            .OfType<Player>()
            .OrderBy(p => Vector3.Distance(p.transform.position, enemy.transform.position))
            .FirstOrDefault();
    }

    // Line of sight check
    public static bool HasLineOfSight(Enemy enemy, Player target)
    {
        if (target == null)
            return false;

        Vector3 origin = enemy.transform.position + Vector3.up * 0.5f;
        Vector3 dir = (target.transform.position - origin).normalized;
        float dist = Vector3.Distance(origin, target.transform.position);

        if (Physics.Raycast(origin, dir, out RaycastHit hit, dist))
        {
            return hit.collider.GetComponent<Player>() != null;
        }

        return false;
    }

    // Apply aim error
    public static Vector3 ApplyAimError(Vector3 direction, float maxAngle)
    {
        float angle = Random.Range(-maxAngle, maxAngle);
        return Quaternion.Euler(0, angle, 0) * direction;
    }

    // Estimate shot range
    public static float EstimateShotRange(Enemy enemy)
    {
        if (enemy.clickAndFlingComp.projectile == null)
            return 0f;

        Projectile proj = enemy.clickAndFlingComp.projectile;

        float shotStrength = enemy.GetStat(ShipStatType.ShotStrength);
        float mass = proj.GetBaseStat(ProjectileStatType.Mass);

        if (mass <= 0) mass = 1f;

        float drag = proj.linearDamping;

        float initialVelocity = shotStrength / mass;

        float estimatedRange = initialVelocity / drag;

        return estimatedRange;
    }

    // Estimate movement range
    public static float EstimateMoveRange(Enemy enemy)
    {
        float moveStrength = enemy.GetStat(ShipStatType.MoveStrength);
        float mass = enemy.GetStat(ShipStatType.Mass);

        if (mass <= 0) mass = 1f;

        float drag = enemy.GetComponent<Rigidbody>().linearDamping;

        float velocity = moveStrength / mass;

        return (velocity / drag);
    }

    // Obstacle avoidance steering
    public static Vector3 GetSteeredDirection(Enemy enemy, Vector3 desiredDirection, float checkDistance = 5f)
    {
        Vector3 origin = enemy.transform.position + Vector3.up * 0.5f;

        if (!Physics.Raycast(origin, desiredDirection, checkDistance))
            return desiredDirection;

        Vector3 left = Quaternion.Euler(0, -45, 0) * desiredDirection;
        if (!Physics.Raycast(origin, left, checkDistance))
            return left;

        Vector3 right = Quaternion.Euler(0, 45, 0) * desiredDirection;
        if (!Physics.Raycast(origin, right, checkDistance))
            return right;

        Vector3 hardLeft = Quaternion.Euler(0, -90, 0) * desiredDirection;
        if (!Physics.Raycast(origin, hardLeft, checkDistance))
            return hardLeft;

        Vector3 hardRight = Quaternion.Euler(0, 90, 0) * desiredDirection;
        if (!Physics.Raycast(origin, hardRight, checkDistance))
            return hardRight;

        return desiredDirection;
    }

    // Orbit movement logic
    public static Vector3 GetOrbitDirection(
        Enemy enemy,
        Player target,
        float desiredDistance,
        float orbitStrength = 1f)
    {
        Vector3 toPlayer = target.transform.position - enemy.transform.position;
        toPlayer.y = 0;

        float distance = toPlayer.magnitude;

        Vector3 radialDir = toPlayer.normalized;

        // perpendicular direction
        Vector3 orbitDir = Vector3.Cross(Vector3.up, radialDir);

        orbitDir *= enemy.orbitSide;

        if (distance > desiredDistance)
            radialDir = radialDir;
        else if (distance < desiredDistance * 0.8f)
            radialDir = -radialDir;
        else
            radialDir = Vector3.zero;

        Vector3 finalDir = (radialDir + orbitDir * orbitStrength).normalized;

        return finalDir;
    }
}