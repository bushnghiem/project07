using System.Collections.Generic;
using UnityEngine;

public class TurnOrderUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BattleManager battleManager;

    [SerializeField] private Transform contentRoot;

    [SerializeField] private TurnOrderEntryUI entryPrefab;

    private readonly List<TurnOrderEntryUI> entries = new();

    private void OnEnable()
    {
        TurnEvent.OnUnitTurnStart += HandleTurnChanged;

        SpawnEvent.OnUnitSpawned += HandleUnitSpawned;

        DeathEvent.OnEntityDeath += HandleEntityDeath;
    }

    private void OnDisable()
    {
        TurnEvent.OnUnitTurnStart -= HandleTurnChanged;

        SpawnEvent.OnUnitSpawned -= HandleUnitSpawned;

        DeathEvent.OnEntityDeath -= HandleEntityDeath;
    }

    private void Start()
    {
        Refresh();
    }

    private void HandleTurnChanged(Unit unit)
    {
        Refresh();
    }

    private void HandleUnitSpawned(Unit unit)
    {
        Refresh();
    }

    private void HandleEntityDeath(Entity entity)
    {
        Refresh();
    }

    public void Refresh()
    {
        ClearEntries();

        List<Unit> units = battleManager.GetTurnOrder();

        foreach (Unit unit in units)
        {
            bool isCurrentTurn =
                unit == battleManager.currentUnit;

            TurnOrderEntryUI entry =
                Instantiate(
                    entryPrefab,
                    contentRoot
                );

            entry.Setup(unit, isCurrentTurn);

            entries.Add(entry);
        }
    }

    private void ClearEntries()
    {
        foreach (var entry in entries)
        {
            if (entry != null)
            {
                Destroy(entry.gameObject);
            }
        }

        entries.Clear();
    }
}