using UnityEngine;

public abstract class EnemyAIBase : MonoBehaviour
{
    public BattleManager battleManager;

    public abstract void TakeTurn(Enemy enemy);
}