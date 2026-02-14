using UnityEngine;

public abstract class BattleState
{
    protected BattleManager manager;

    public BattleState(BattleManager manager)
    {
        this.manager = manager;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
