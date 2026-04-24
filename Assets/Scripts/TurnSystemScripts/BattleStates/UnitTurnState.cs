public class UnitTurnState : BattleState
{
    private bool isNewTurn;

    public UnitTurnState(BattleManager manager, bool isNewTurn = true) : base(manager)
    {
        this.isNewTurn = isNewTurn;
    }

    public override void Enter()
    {
        if (isNewTurn)
        {
            manager.currentUnit.StartTurn();
        }
    }
}