using UnityEngine;
using System.Collections;

public class EnemyTurnState : BattleState
{
    public EnemyTurnState(BattleManager manager) : base(manager) { }

    public override void Enter()
    {
        Debug.Log(manager.enemy);
        if (!manager.enemy.isDead)
        {
            manager.StartCoroutine(EnemyRoutine());
        }
    }

    private IEnumerator EnemyRoutine()
    {
        Debug.Log("Enemy Thinking...");
        yield return new WaitForSeconds(1f);

        if (!manager.enemy.isDead)
        {
            manager.enemy.Attack(manager.player);
        }
    }
}

