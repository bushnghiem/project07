using UnityEngine;

public class IdleState : EnemyState
{
    public override UnitAction DecideAction(
        Enemy enemy,
        BattleManager battleManager)
    {
        return null;
    }
}