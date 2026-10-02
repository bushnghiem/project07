using UnityEngine;
using System.Collections.Generic;

public class StatusEffectController : MonoBehaviour
{
    private List<StatusEffectInstance> activeEffects = new();

    public IReadOnlyList<StatusEffectInstance> ActiveEffects =>
        activeEffects;

    private Unit unit;

    public event System.Action OnEffectsChanged;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe(OnUnitEvent);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(OnUnitEvent);
    }

    public void ApplyEffect(
        StatusEffectData data,
        int stacks)
    {
        if (data == null)
            return;

        var existing = activeEffects.Find(
            e => e.data == data
        );

        if (existing != null)
        {
            if (data.isStackable)
            {
                existing.SetStacks(
                    Mathf.Min(
                        existing.Stacks + stacks,
                        data.maxStacks
                    )
                );
            }

            existing.RefreshDuration();

            OnEffectsChanged?.Invoke();

            return;
        }

        var instance = data.CreateInstance(unit);

        instance.Init(
            data,
            unit,
            stacks
        );

        instance.SetModifierChangedCallback(
            HandleEffectModifiersChanged
        );

        activeEffects.Add(instance);

        instance.OnApply();

        OnEffectsChanged?.Invoke();

        Debug.Log(
            $"Applied effect: {data.name} | " +
            $"stacks={stacks} on {unit}"
        );
    }

    private void HandleEffectModifiersChanged()
    {
        OnEffectsChanged?.Invoke();
    }

    public void RemoveEffect(
        StatusEffectInstance effect)
    {
        if (effect == null)
            return;

        if (!activeEffects.Remove(effect))
            return;

        effect.OnRemove();

        OnEffectsChanged?.Invoke();
    }

    public bool HasEffect(StatusEffectData data)
    {
        return activeEffects.Exists(
            e => e.data == data
        );
    }

    public StatusEffectInstance GetEffect(
        StatusEffectData data)
    {
        return activeEffects.Find(
            e => e.data == data
        );
    }

    public bool RemoveEffect(StatusEffectData data)
    {
        var effect = GetEffect(data);

        if (effect == null)
            return false;

        RemoveEffect(effect);

        return true;
    }

    public IEnumerable<StatModifier> GetStatModifiers()
    {
        foreach (var effect in activeEffects)
        {
            foreach (var modifier in effect.StatModifiers)
            {
                yield return modifier;
            }
        }
    }

    public float ModifyIncomingDamage(
        DamageInfo damageInfo,
        float damage)
    {
        float result = damage;

        foreach (var effect in activeEffects)
        {
            result = effect.ModifyIncomingDamage(
                damageInfo,
                result
            );
        }

        return result;
    }

    private void OnUnitEvent(UnitEvent e)
    {
        if (e.source != unit &&
            e.target != unit)
            return;

        if (e.type == UnitEventType.TurnStart ||
            e.type == UnitEventType.TurnEnd)
            return;

        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].OnEvent(e);
        }
    }

    public void OnTurnStart()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].OnTurnStart();
        }
    }

    public void OnTurnEnd()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            var effect = activeEffects[i];

            effect.OnTurnEnd();
            effect.TickDuration();

            if (effect.IsExpired)
            {
                RemoveEffect(effect);
            }
        }
    }
}
