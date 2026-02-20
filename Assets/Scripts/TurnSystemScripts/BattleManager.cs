using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private BattleState currentState;

    public Unit currentUnit;
    private int currentIndex = 0;
    private int currentRound = 0;

    public List<Unit> allUnits = new List<Unit>();
    public List<Unit> allPlayers = new List<Unit>();
    public List<Unit> allEnemies = new List<Unit>();

    private void OnEnable()
    {
        DeathEvent.OnEntityDeath += HandleBattleDeath;
        TurnEvent.OnPlayerTurnEnd += HandlePlayerTurnEnd;
        TurnEvent.OnEnemyTurnEnd += HandleEnemyTurnEnd;
        TurnEvent.OnUnitTurnEnd += HandleUnitTurnEnd;
        SpawnEvent.OnUnitSpawned += HandleUnitSpawned;
    }

    private void OnDisable()
    {
        DeathEvent.OnEntityDeath -= HandleBattleDeath;
        TurnEvent.OnPlayerTurnEnd -= HandlePlayerTurnEnd;
        TurnEvent.OnEnemyTurnEnd -= HandleEnemyTurnEnd;
        TurnEvent.OnUnitTurnEnd -= HandleUnitTurnEnd;
        SpawnEvent.OnUnitSpawned -= HandleUnitSpawned;
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

    public void RoundStart()
    {
        currentRound += 1;
        Debug.Log(currentRound);
        SortByInitiative();
        currentIndex = 0;
        currentUnit = allUnits[currentIndex];
    }

    public void SortByInitiative()
    {
        allUnits = allUnits
        .OrderByDescending(u => u.Initiative)       // highest initiative first
        .ThenBy(u => u.IsPlayerControllable == true ? 0 : 1) // Player units first on tie
        .ToList();
    }

    public void RemoveDeadUnit(Unit unit)
    {
        int index = allUnits.IndexOf(unit);

        if (index == -1) return;

        // If the removed unit is BEFORE the current index,
        // we must shift the index back
        if (index < currentIndex)
        {
            currentIndex--;
        }

        allUnits.RemoveAt(index);

        if (unit.IsPlayerControllable)
            allPlayers.Remove(unit);
        else
            allEnemies.Remove(unit);

    }

    public bool checkWin()
    {
        if (allEnemies.Count == 0)
        {
            return true;
        }
        return false;
    }

    public bool checkLoss()
    {
        if (allPlayers.Count == 0)
        {
            return true;
        }
        return false;
    }

    private IEnumerator WaitForBattlefieldToSettle(Unit actingUnit)
    {
        //Debug.Log("Waiting for battlefield to settle...");

        float stillTimer = 0f;
        const float requiredStillTime = 0.5f;

        while (stillTimer < requiredStillTime)
        {
            if (IsAnythingMoving())
            {
                //Debug.Log("Something moving...");
                stillTimer = 0f;
            }
            else
            {
                stillTimer += Time.fixedDeltaTime;
            }

            yield return new WaitForFixedUpdate();
        }

        //Debug.Log("Battlefield settled. Ending turn.");
        EndOfTurn(actingUnit);
    }

    private bool IsAnythingMoving()
    {
        Rigidbody[] bodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

        const float velocityThreshold = 0.05f;
        const float angularThreshold = 0.05f;

        foreach (var rb in bodies)
        {
            if (rb.isKinematic) continue;

            if (rb.linearVelocity.sqrMagnitude > velocityThreshold * velocityThreshold)
                return true;

            if (rb.angularVelocity.sqrMagnitude > angularThreshold * angularThreshold)
                return true;
        }

        return false;
    }

    private void EndOfTurn(Unit unit)
    {
        if (checkLoss())
        {
            SwitchState(new LoseState(this));
            return;
        }
        if (checkWin())
        {
            SwitchState(new WinState(this));
            return;
        }
        currentIndex++;
        if (currentIndex >= allUnits.Count)
        {
            SwitchState(new StartState(this));
        }
        else
        {
            currentUnit = allUnits[currentIndex];
            SwitchState(new UnitTurnState(this));
        }
    }

    private void HandleBattleDeath(Entity deadEntity)
    {
        if (deadEntity is Unit)
        {
            RemoveDeadUnit((Unit)deadEntity);
        }
    }

    public void HandlePlayerTurnEnd(Entity player)
    {
        Debug.Log(player + " has ended turn");
        //SwitchState(new EnemyTurnState(this));
    }

    public void HandleEnemyTurnEnd(Entity enemy)
    {
        Debug.Log(enemy + " has ended turn");
        //SwitchState(new PlayerTurnState(this));
    }

    public void HandleUnitTurnEnd(Unit unit)
    {
        StartCoroutine(WaitForBattlefieldToSettle(unit));
    }

    public void HandleUnitSpawned(Unit unit)
    {
        Debug.Log("Added " +  unit);
        allUnits.Add(unit);
        if (unit.IsPlayerControllable)
        {
            allPlayers.Add(unit);
            Debug.Log(allPlayers);
        }
        else
        {
            allEnemies.Add(unit);
            Debug.Log(allEnemies);
        }
    }
}