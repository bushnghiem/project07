using UnityEngine;

public class IdleState : EnemyState
{
    public override UnitAction DecideAction(
        Enemy enemy,
        StateMachineAI ai)
    {
        return null;
    }
}