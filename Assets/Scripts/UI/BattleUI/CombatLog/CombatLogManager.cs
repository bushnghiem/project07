using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatLogManager : MonoBehaviour
{
    public static CombatLogManager Instance { get; private set; }

    private readonly List<CombatLogEntry> entries = new();

    public IReadOnlyList<CombatLogEntry> Entries => entries;

    public event Action<CombatLogEntry> OnLogAdded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        EventBus.Subscribe(HandleUnitEvent);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(HandleUnitEvent);
    }

    private void HandleUnitEvent(UnitEvent evt)
    {
        string message = BuildMessage(evt);

        AddMessage(message);
    }

    public void AddMessage(string message)
    {
        CombatLogEntry entry = new(message);

        entries.Add(entry);

        OnLogAdded?.Invoke(entry);

        //Debug.Log($"[Combat Log] {message}");
    }

    private string BuildMessage(UnitEvent evt)
    {
        string sourceName;

        if (evt.source != null)
            sourceName = evt.source.DisplayName;
        else if (evt.damageSource != null)
            sourceName = evt.damageSource.DisplayName;
        else
            sourceName = "Unknown";

        string targetName =
            evt.target != null
            ? evt.target.RunData.uniqueID
            : "Unknown";

        switch (evt.type)
        {
            case UnitEventType.TurnStart:
                return $"{sourceName} begins its turn.";

            case UnitEventType.Move:
                return $"{sourceName} moved.";

            case UnitEventType.Shoot:
                return $"{sourceName} fired.";

            case UnitEventType.Hurt:
                return $"{sourceName} dealt {evt.value:0} damage to {targetName}.";

            case UnitEventType.Heal:
                return $"{sourceName} healed {evt.value:0} HP.";

            case UnitEventType.Shield:
                return $"{sourceName} gained {evt.value:0} shield.";

            case UnitEventType.ItemUse:
                return $"{sourceName} used an item.";

            case UnitEventType.Death:
                return $"{sourceName} was destroyed.";

            case UnitEventType.TurnEnd:
                return $"{sourceName} ended its turn.";

            default:
                return evt.type.ToString();
        }
    }
}