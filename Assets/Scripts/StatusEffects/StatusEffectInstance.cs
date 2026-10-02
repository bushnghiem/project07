using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class StatusEffectInstance
{
    public StatusEffectData data { get; private set; }
    public Unit target { get; private set; }

    public int Stacks { get; private set; }
    public int RemainingDuration { get; private set; }

    private readonly List<StatModifier> statModifiers = new();

    public IReadOnlyList<StatModifier> StatModifiers =>
        statModifiers;

    private Action onModifiersChanged;

    public void SetModifierChangedCallback(Action callback)
    {
        onModifiersChanged = callback;
    }

    protected void NotifyModifiersChanged()
    {
        onModifiersChanged?.Invoke();
    }

    public void SetStacks(int value)
    {
        Stacks = Mathf.Clamp(value, 0, data.maxStacks);

        UpdateStatModifiers();
        NotifyModifiersChanged();
    }

    public void AddStacks(int amount)
    {
        Stacks = Mathf.Clamp(
            Stacks + amount,
            0,
            data.maxStacks
        );

        UpdateStatModifiers();
        NotifyModifiersChanged();
    }

    public void RemoveStacks(int amount)
    {
        Stacks = Mathf.Max(
            0,
            Stacks - amount
        );

        UpdateStatModifiers();
        NotifyModifiersChanged();
    }

    public void SetDuration(int value)
    {
        RemainingDuration = value;
    }

    public void RefreshDuration()
    {
        RemainingDuration = data.duration;
    }

    public virtual void OnApply() { }

    public virtual void OnRemove()
    {
        statModifiers.Clear();
        NotifyModifiersChanged();
    }

    public virtual void OnTurnStart() { }

    public virtual void OnTurnEnd() { }

    public virtual void OnEvent(UnitEvent e) { }

    public virtual float ModifyIncomingDamage(
        DamageInfo damageInfo,
        float damage)
    {
        return damage;
    }

    public virtual void TickDuration()
    {
        RemainingDuration--;
    }

    public bool IsExpired =>
        RemainingDuration <= 0;

    public void Init(
        StatusEffectData data,
        Unit target,
        int stacks)
    {
        this.data = data;
        this.target = target;

        Stacks = Mathf.Clamp(
            stacks,
            0,
            data.maxStacks
        );

        RemainingDuration = data.duration;
    }

    protected virtual void UpdateStatModifiers()
    {
    }

    protected void SetStatModifier(
        ShipStatType statType,
        float flatBonus,
        float percentBonus = 0f)
    {
        StatModifier existing = statModifiers.Find(
            m => m.statType == statType
        );

        if (existing != null)
        {
            existing.flatBonus = flatBonus;
            existing.percentBonus = percentBonus;
            existing.sourceID = data.effectID;
        }
        else
        {
            statModifiers.Add(new StatModifier
            {
                statType = statType,
                flatBonus = flatBonus,
                percentBonus = percentBonus,
                sourceID = data.effectID
            });
        }
    }

    protected void RemoveStatModifier(
        ShipStatType statType)
    {
        statModifiers.RemoveAll(
            m => m.statType == statType
        );
    }

    protected void ClearStatModifiers()
    {
        statModifiers.Clear();
    }
}
