using UnityEngine;

public class IdleState : EnemyState
{
    public override void Execute(Enemy enemy, StateMachineAI ai)
    {
        Debug.Log("Idle fallback");
        enemy.EndTurn();
    }
}