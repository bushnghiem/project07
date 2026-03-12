using UnityEngine;

public class DefaultAI : EnemyAIBase
{
    [Header("References")]
    public BattleManager battleManager;

    [Header("Fling Settings")]
    public float shootPower = 0.8f;
    public float movePower = 0.6f;

    public override void TakeTurn(Enemy enemy)
    {
        if (battleManager == null)
        {
            Debug.LogWarning($"[{enemy.name}] No BattleManager assigned to AI!");
            enemy.EndTurn();
            return;
        }

        var target = EnemyAIUtility.GetClosestPlayer(enemy, battleManager);
        if (target == null)
        {
            enemy.EndTurn();
            return;
        }

        Vector3 direction = (target.transform.position - enemy.transform.position);
        direction.y = 0;
        direction.Normalize();

        bool canShoot = EnemyAIUtility.HasLineOfSight(enemy, target);

        if (canShoot)
            Shoot(enemy, direction);
        else
            Move(enemy, direction);
    }

    private void Shoot(Enemy enemy, Vector3 direction)
    {
        enemy.clickAndFlingComp.SetFlingable(true);
        enemy.clickAndFlingComp.SetProjectileMode(true);
        enemy.clickAndFlingComp.ExecuteFling(direction, shootPower);

        enemy.Shoot();
        enemy.EndTurn();
    }

    private void Move(Enemy enemy, Vector3 direction)
    {
        enemy.clickAndFlingComp.SetFlingable(true);
        enemy.clickAndFlingComp.SetProjectileMode(false);
        enemy.clickAndFlingComp.ExecuteFling(direction, movePower);

        enemy.Move();
        enemy.EndTurn();
    }
}