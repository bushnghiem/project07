using UnityEngine;

public abstract class QuestObjective : ScriptableObject
{
    public abstract void PlaceObjective(
        GridManager grid,
        QuestInstance quest);

    public abstract bool TryComplete(
        QuestInstance quest,
        TileData tile);

    public virtual string HandleEvent(
        QuestInstance quest,
        QuestEvent questEvent)
    {
        return null;
    }

    public virtual string GetProgressText(
        QuestInstance quest,
        GridManager grid)
    {
        return "Complete the objective";
    }

    public virtual void OnQuestStarted(
        QuestInstance quest,
        System.Action onComplete)
    {
        onComplete?.Invoke();
    }
}