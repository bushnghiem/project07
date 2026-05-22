using UnityEngine;

public abstract class EnemyState
{
    public abstract UnitAction DecideAction(
        Enemy enemy,
        EnemyAIBase ai
    );
}