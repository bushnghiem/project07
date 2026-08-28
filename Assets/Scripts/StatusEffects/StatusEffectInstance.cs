using UnityEngine;

public abstract class StatusEffectInstance
{
    public StatusEffectData data { get; private set; }
    public Unit target { get; private set; }

    public int Stacks { get; private set; }
    public int RemainingDuration { get; private set; }

    public void SetStacks(int value)
    {
        Stacks = value;
    }

    public void SetDuration(int value)
    {
        RemainingDuration = value;
    }

    public void AddStacks(int amount)
    {
        Stacks = Mathf.Clamp(
            Stacks + amount,
            0,
            data.maxStacks
        );
    }

    public void RemoveStacks(int amount)
    {
        Stacks = Mathf.Max(0, Stacks - amount);
    }

    public void RefreshDuration()
    {
        RemainingDuration = data.duration;
    }

    public virtual void OnApply() { }
    public virtual void OnRemove() { }

    public virtual void OnTurnStart() { }
    public virtual void OnTurnEnd() { }

    public virtual void OnEvent(UnitEvent e) { }

    public virtual float ModifyStat(
        ShipStatType statType,
        float value)
    {
        return value;
    }

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

    public bool IsExpired => RemainingDuration <= 0;

    public void Init(StatusEffectData data, Unit target, int stacks)
    {
        this.data = data;
        this.target = target;

        Stacks = stacks;
        RemainingDuration = data.duration;
    }
}
