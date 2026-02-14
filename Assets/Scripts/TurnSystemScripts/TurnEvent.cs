using System;

public static class TurnEvent
{
    public static Action<Unit> OnPlayerTurnStart;
    public static Action<Unit> OnEnemyTurnStart;
    public static Action<Unit> OnPlayerTurnEnd;
    public static Action<Unit> OnEnemyTurnEnd;
}