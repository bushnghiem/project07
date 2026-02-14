using UnityEngine;

public interface TurnAction
{
    string ActionName { get; }
    void Execute(Unit unit);
}