using UnityEngine;
using System.Collections.Generic;

public class StatusEffectController : MonoBehaviour
{
    private List<StatusEffectInstance> activeEffects = new();
    private Unit unit;

    void Awake()
    {
        unit = GetComponent<Unit>();
        Debug.Log($"StatusEffectController attached to {unit}");
    }

    void OnEnable()
    {
        EventBus.Subscribe(OnUnitEvent);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(OnUnitEvent);
    }

    public void ApplyEffect(StatusEffectData data, int stacks)
    {
        var existing = activeEffects.Find(e => e.data == data);

        if (existing != null)
        {
            if (data.isStackable)
            {
                existing.SetStacks(
                    Mathf.Min(existing.Stacks + stacks, data.maxStacks)
                );
            }

            existing.SetDuration(data.duration);
            return;
        }

        var instance = data.CreateInstance(unit);
        instance.Init(data, unit, stacks);

        activeEffects.Add(instance);
        instance.OnApply();

        Debug.Log($"Applied effect: {data.name} | stacks={stacks} on {unit}");
    }

    public void RemoveEffect(StatusEffectInstance effect)
    {
        effect.OnRemove();
        activeEffects.Remove(effect);
    }

    void OnUnitEvent(UnitEvent e)
    {
        if (e.source != unit && e.target != unit) return;

        if (e.type == UnitEventType.TurnStart || e.type == UnitEventType.TurnEnd)
            return;

        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].OnEvent(e);
        }
    }

    public void OnTurnStart()
    {
        Debug.Log("Call start Turn");

        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].OnTurnStart();
        }
    }

    public void OnTurnEnd()
    {
        Debug.Log("Call end Turn");

        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            var effect = activeEffects[i];

            effect.OnTurnEnd();
            effect.TickDuration();

            if (effect.IsExpired)
                RemoveEffect(effect);
        }
    }
}