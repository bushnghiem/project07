using System;

public static class TurnEvent
{
    public static Action<Unit> OnUnitTurnStart; // Unit
    public static Action<Unit> OnUnitTurnEnd; // Unit
    public static Action<Unit> OnNextTurn;
    public static Action OnRoundEnd;
    public static Action OnFightWon;
    public static Action OnFightLost;
}