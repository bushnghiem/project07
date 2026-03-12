using UnityEngine;
using System.Linq;

public static class EnemyAIUtility
{
    // Get the closest player from a specific BattleManager instance
    public static Player GetClosestPlayer(Enemy enemy, BattleManager battleManager)
    {
        if (battleManager == null || battleManager.allPlayers.Count == 0)
            return null;

        return battleManager.allPlayers
            .OfType<Player>()
            .OrderBy(p => Vector3.Distance(p.transform.position, enemy.transform.position))
            .FirstOrDefault();
    }

    // Check line-of-sight to a target player
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
}