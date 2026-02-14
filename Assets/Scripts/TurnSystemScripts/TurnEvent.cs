using System;

public static class TurnEvent
{
    public static Action<Entity> OnPlayerTurnEnd;
    public static Action<Entity> OnEnemyTurnEnd;
}