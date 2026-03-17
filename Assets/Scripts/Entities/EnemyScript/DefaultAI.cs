using UnityEngine;

public class DefaultAI : EnemyAIBase
{
    [Header("References")]
    public BattleManager battleManager;

    [Header("Behavior")]
    public float preferredShootDistancePercent = 0.7f;

    [Header("Accuracy")]
    [Range(0f, 20f)]
    public float aimErrorAngle = 6f;

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

        Vector3 direction = target.transform.position - enemy.transform.position;
        direction.y = 0;
        direction.Normalize();

        float distance = Vector3.Distance(enemy.transform.position, target.transform.position);

        float maxShotRange = EnemyAIUtility.EstimateShotRange(enemy);
        float maxMoveRange = EnemyAIUtility.EstimateMoveRange(enemy);

        bool hasLOS = EnemyAIUtility.HasLineOfSight(enemy, target);
        bool canShoot = hasLOS && distance <= maxShotRange;

        if (canShoot)
        {
            float shootPower = Mathf.Clamp01(distance / maxShotRange);

            shootPower *= Random.Range(0.9f, 1.1f);
            shootPower = Mathf.Clamp01(shootPower);

            float accuracyMultiplier = distance / maxShotRange;
            float error = aimErrorAngle * accuracyMultiplier;

            Vector3 aimDirection = EnemyAIUtility.ApplyAimError(direction, error);

            Shoot(enemy, aimDirection, shootPower);
        }
        else
        {
            float desiredDistance = maxShotRange * preferredShootDistancePercent;

            float neededMove = distance - desiredDistance;

            float movePower = Mathf.Clamp01(neededMove / maxMoveRange);

            movePower *= Random.Range(0.9f, 1.1f);
            movePower = Mathf.Clamp01(movePower);

            Vector3 orbitDir = EnemyAIUtility.GetOrbitDirection(
                enemy,
                target,
                desiredDistance
            );

            orbitDir = EnemyAIUtility.GetSteeredDirection(enemy, orbitDir);

            Move(enemy, orbitDir, movePower);
        }
    }

    private void Shoot(Enemy enemy, Vector3 direction, float power)
    {
        enemy.clickAndFlingComp.SetFlingable(true);
        enemy.clickAndFlingComp.SetProjectileMode(true);

        enemy.clickAndFlingComp.ExecuteFling(direction, power);

        enemy.Shoot();
        enemy.EndTurn();
    }

    private void Move(Enemy enemy, Vector3 direction, float power)
    {
        enemy.clickAndFlingComp.SetFlingable(true);
        enemy.clickAndFlingComp.SetProjectileMode(false);

        enemy.clickAndFlingComp.ExecuteFling(direction, power);

        enemy.Move();
        enemy.EndTurn();
    }
}