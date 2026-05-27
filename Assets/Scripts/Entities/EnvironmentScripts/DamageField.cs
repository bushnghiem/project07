using UnityEngine;
using System.Collections.Generic;

public class DamageField : MonoBehaviour
{
    [Header("Entry Damage")]
    [SerializeField] private bool damageOnEntry = false;
    [SerializeField] private DamageDefinition entryDamage;

    [Header("Tick Damage")]
    [SerializeField] private bool damageOverTime = true;
    [SerializeField] private DamageDefinition tickDamage;

    [SerializeField] private int turnsPerTick = 1;

    private readonly HashSet<Entity> occupants = new();

    private int turnCounter = 0;

    private void OnEnable()
    {
        TurnEvent.OnNextTurn += HandleTurnAdvance;
    }

    private void OnDisable()
    {
        TurnEvent.OnNextTurn -= HandleTurnAdvance;
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity entity = other.GetComponentInParent<Entity>();

        if (entity == null)
            return;

        occupants.Add(entity);

        if (damageOnEntry)
        {
            entity.Hurt(entryDamage.ToDamageInfo());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity entity = other.GetComponentInParent<Entity>();

        if (entity == null)
            return;

        occupants.Remove(entity);
    }

    private void HandleTurnAdvance(Unit unit)
    {
        if (!damageOverTime)
            return;

        turnCounter++;

        if (turnCounter < turnsPerTick)
            return;

        turnCounter = 0;

        ApplyTickDamage();
    }

    private void ApplyTickDamage()
    {
        occupants.RemoveWhere(e => e == null || e.isDead);

        foreach (Entity entity in occupants)
        {
            entity.Hurt(tickDamage.ToDamageInfo());
        }
    }
}