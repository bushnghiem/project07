using UnityEngine;
using System.Linq;

public static class EnemyAIUtility
{
    public static Player GetClosestPlayer(Enemy enemy, BattleManager battleManager)
    {
        if (battleManager == null || battleManager.allPlayers.Count == 0)
            return null;

        return battleManager.allPlayers
            .OfType<Player>()
            .OrderBy(p => Vector3.Distance(p.transform.position, enemy.transform.position))
            .FirstOrDefault();
    }

    public static bool HasLineOfSight(Enemy enemy, Player target)
    {
        if (target == null)
            return false;

        Vector3 origin = enemy.transform.position + Vector3.up * 0.5f;
        Vector3 dir = (target.transform.position - origin).normalized;
        float dist = Vector3.Distance(origin, target.transform.position);

        if (Physics.Raycast(origin, dir, out RaycastHit hit, dist))
        {
            return hit.collider.transform.root.GetComponent<Player>() != null;
        }

        return false;
    }

    public static Vector3 ApplyAimError(Vector3 direction, float maxAngle)
    {
        float angle = Random.Range(-maxAngle, maxAngle);
        return Quaternion.Euler(0, angle, 0) * direction;
    }

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

    public static float EstimateMoveRange(Enemy enemy)
    {
        float moveStrength = enemy.GetStat(ShipStatType.MoveStrength);
        float mass = enemy.GetStat(ShipStatType.Mass);

        if (mass <= 0) mass = 1f;

        float drag = enemy.GetComponent<Rigidbody>().linearDamping;

        float velocity = moveStrength / mass;

        return (velocity / drag);
    }

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

    public static Vector3 GetPlanetAvoidance(Enemy enemy, float avoidRadius = 15f, float strength = 6f)
    {
        Vector3 avoidance = Vector3.zero;
        var planets = GameObject.FindGameObjectsWithTag("Planet");

        foreach (var planet in planets)
        {
            Vector3 toPlanet = planet.transform.position - enemy.Position;
            float distance = toPlanet.magnitude;

            if (distance < avoidRadius)
            {
                float weight = 1f - (distance / avoidRadius);
                avoidance -= toPlanet.normalized * weight * strength;
            }
        }

        return avoidance;
    }

    public static Vector3 GetUnitAvoidance(Enemy enemy, float avoidRadius = 5f, float strength = 2f)
    {
        Vector3 avoidance = Vector3.zero;
        Collider[] hits = Physics.OverlapSphere(enemy.Position, avoidRadius);

        foreach (var hit in hits)
        {
            if (hit.gameObject == enemy.gameObject) continue;

            Unit other = hit.GetComponent<Unit>();
            if (other == null) continue;

            Vector3 toOther = enemy.Position - other.Position;
            float distance = toOther.magnitude;

            if (distance > 0.01f)
            {
                float weight = Mathf.Clamp01((avoidRadius - distance) / avoidRadius);
                avoidance += toOther.normalized * weight * strength;
            }
        }

        return avoidance;
    }

    public static bool IsShotBlockedByAlly(Enemy enemy, Vector3 direction, float maxDistance)
    {
        Vector3 origin = enemy.transform.position + Vector3.up * 0.5f;

        RaycastHit[] hits = Physics.SphereCastAll(
            origin,
            0.5f,
            direction,
            maxDistance
        );

        foreach (var hit in hits)
        {
            if (hit.collider.transform.root == enemy.transform)
                continue;

            if (hit.collider.GetComponentInParent<Enemy>() != null)
                return true;
        }

        return false;
    }

    public static Vector3 PredictGravityCompensatedDirection(Enemy enemy, Vector3 targetPos, int steps = 100, float dt = 0.05f)
    {
        Vector3 origin = enemy.Position;
        float shotStrength = enemy.GetStat(ShipStatType.ShotStrength); ;

        var gravityFields = GameObject.FindGameObjectsWithTag("Gravity")
            .Select(g => g.GetComponent<GravityPullComponent>())
            .Where(g => g != null)
            .ToArray();

        var planets = GameObject.FindGameObjectsWithTag("Planet")
            .Select(p => p.transform.position)
            .ToArray();

        Vector3 targetDir = (targetPos - origin).normalized;
        Vector3 bestDir = targetDir;
        float bestScore = float.MinValue;

        for (int angleStep = -5; angleStep <= 5; angleStep++)
        {
            float angleOffset = angleStep * 2f;
            Vector3 candidateDir = Quaternion.Euler(0, angleOffset, 0) * targetDir;

            Vector3 pos = origin;
            Vector3 vel = candidateDir * shotStrength;
            bool blocked = false;

            for (int i = 0; i < steps; i++)
            {
                foreach (var g in gravityFields)
                {
                    Vector3 dir = g.transform.parent.position - pos;
                    float dist = dir.magnitude;
                    if (dist > 0.01f)
                        vel += dir.normalized * g.gravityStrength / dist * dt;
                }

                Vector3 nextPos = pos + vel * dt;

                foreach (var planet in planets)
                {
                    if ((nextPos - planet).magnitude < 1f)
                    {
                        blocked = true;
                        break;
                    }
                }

                if (blocked) break;

                pos = nextPos;

                if ((pos - targetPos).magnitude < 0.5f) break;
            }

            if (!blocked)
            {
                float score = 1f / Vector3.Distance(pos, targetPos);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestDir = candidateDir;
                }
            }
        }

        return bestDir;
    }
}