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

    public virtual void OnApply() { }
    public virtual void OnRemove() { }

    public virtual void OnTurnStart() { }
    public virtual void OnTurnEnd() { }

    public virtual void OnEvent(UnitEvent e) { }

    public virtual void TickDuration()
    {
        RemainingDuration--;
    }

    public bool IsExpired => RemainingDuration <= 0;

    public void Init(StatusEffectData data, Unit target, int stacks)
    {
        this.data = data;
        this.target = target;

        this.Stacks = stacks;
        this.RemainingDuration = data.duration;
    }
}