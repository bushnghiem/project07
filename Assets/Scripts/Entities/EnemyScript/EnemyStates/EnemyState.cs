using UnityEngine;

public abstract class EnemyState
{
    public abstract UnitAction DecideAction(
        Enemy enemy,
        StateMachineAI ai
    );
}