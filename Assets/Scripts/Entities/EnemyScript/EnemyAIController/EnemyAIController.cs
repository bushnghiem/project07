using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    [SerializeField] private EnemyAIBehavior defaultBehavior;

    private EnemyAIBehavior currentBehavior;

    private Enemy enemy;
    private BattleManager battleManager;
    private UnitActionExecutor executor;

    private EnemyAIContext context;

    public EnemyAIBehavior CurrentBehavior => currentBehavior;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();

        battleManager = FindFirstObjectByType<BattleManager>();
        executor = FindFirstObjectByType<UnitActionExecutor>();

        context = new EnemyAIContext();
    }

    public void SetBehavior(EnemyAIBehavior behavior)
    {
        currentBehavior = behavior;
    }

    public void InitializeFromRunData(
    ShipRunData data,
    AIDatabase database)
    {
        if (database == null)
        {
            Debug.LogError("Missing AI Database");

            currentBehavior = defaultBehavior;
            return;
        }

        EnemyAIBehavior loaded =
            database.Get(data.aiBehaviorID);

        if (loaded == null)
        {
            Debug.LogWarning($"AI Behavior '{data.aiBehaviorID}' not found");

            currentBehavior = defaultBehavior;
            return;
        }

        currentBehavior = loaded;
    }

    public void TakeTurn()
    {
        if (currentBehavior == null)
        {
            enemy.EndTurn();
            return;
        }

        UnitAction action = currentBehavior.DecideAction(enemy, battleManager, context);

        if (action == null)
        {
            enemy.EndTurn();
            return;
        }

        executor.Execute(action);
    }
}