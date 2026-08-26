using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/Quest")]
public class QuestData : ScriptableObject
{
    public int questId;

    public string questName;

    [TextArea]
    public string description;

    public QuestObjective objective;

    public int floorsAhead;

    public List<RewardDefinition> rewards;

    public int rewardsToChoose = 1;

    public Item deliveryItem;
}
