using UnityEngine;

public abstract class EnemyAIBase : MonoBehaviour
{
    public abstract void TakeTurn(Enemy enemy);
}