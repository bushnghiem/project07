using System;

public static class TurnEvent
{
    public static Action<Unit> OnPlayerTurnStart;
    public static Action<Unit> OnEnemyTurnStart;
    public static Action<Unit> OnPlayerTurnEnd;
    public static Action<Unit> OnEnemyTurnEnd;
    public static Action<Unit> OnUnitTurnStart;
    public static Action<Unit> OnUnitTurnEnd;
    public static Action OnRoundEnd;
    public static Action OnFightWon;
    public static Action OnFightLost;
}