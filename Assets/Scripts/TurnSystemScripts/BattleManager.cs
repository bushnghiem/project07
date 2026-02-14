using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private BattleState currentState;

    public Player player;
    public Enemy enemy;

    private void OnEnable()
    {
        DeathEvent.OnEntityDeath += HandleBattleDeath;
        TurnEvent.OnPlayerTurnEnd += HandlePlayerTurnEnd;
        TurnEvent.OnEnemyTurnEnd += HandleEnemyTurnEnd;
    }

    private void OnDisable()
    {
        DeathEvent.OnEntityDeath -= HandleBattleDeath;
        TurnEvent.OnPlayerTurnEnd -= HandlePlayerTurnEnd;
        TurnEvent.OnEnemyTurnEnd -= HandleEnemyTurnEnd;
    }

    private void HandleBattleDeath(Entity deadEntity)
    {
        if (deadEntity is Player)
            SwitchState(new LoseState(this));
        else if (deadEntity is Enemy)
            SwitchState(new WinState(this));
    }

    private void Start()
    {
        SwitchState(new StartState(this));
    }

    private void Update()
    {
        currentState?.Update();
    }

    public void SwitchState(BattleState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void HandlePlayerTurnEnd(Entity player)
    {
        Debug.Log(player + " has ended turn");
        SwitchState(new EnemyTurnState(this));
    }

    public void HandleEnemyTurnEnd(Entity enemy)
    {
        Debug.Log(enemy + " has ended turn");
        SwitchState(new PlayerTurnState(this));
    }
}