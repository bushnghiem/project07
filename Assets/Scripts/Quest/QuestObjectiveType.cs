using UnityEngine;

public abstract class QuestObjective : ScriptableObject
{
    public abstract void PlaceObjective(
        GridManager grid,
        QuestInstance quest);

    public abstract bool TryComplete(
        QuestInstance quest,
        TileData tile);

    public virtual void OnQuestStarted(
        QuestInstance quest,
        System.Action onComplete)
    {
        onComplete?.Invoke();
    }
}


