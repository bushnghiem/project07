using UnityEngine;

public abstract class EnemyState
{
    public abstract void Execute(Enemy enemy, StateMachineAI ai);
}