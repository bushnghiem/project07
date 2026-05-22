using UnityEngine;

public class IdleState : EnemyState
{
    public override UnitAction DecideAction(Enemy enemy, EnemyAIBase ai)
    {
        return null;
    }
}