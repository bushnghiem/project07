using UnityEngine;

public abstract class EnemyState
{
    public abstract UnitAction DecideAction(
        Enemy enemy,
        BattleManager battleManager
    );
}