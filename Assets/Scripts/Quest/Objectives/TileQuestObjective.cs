using UnityEngine;

public abstract class TileQuestObjective : QuestObjective
{
    public override string HandleEvent(
        QuestInstance quest,
        QuestEvent questEvent)
    {
        if (questEvent.type == QuestEventType.TileCorrupted &&
            questEvent.position == quest.targetPosition)
        {
            return GetCorruptionFailureReason(quest);
        }

        return HandleSpecificEvent(quest, questEvent);
    }

    protected virtual string GetCorruptionFailureReason(
        QuestInstance quest)
    {
        return "Corruption overtook the objective location.";
    }

    protected virtual string HandleSpecificEvent(
        QuestInstance quest,
        QuestEvent questEvent)
    {
        return null;
    }
}
