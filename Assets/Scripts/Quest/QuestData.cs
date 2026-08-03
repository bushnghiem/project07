using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Quest")]
public class QuestData : ScriptableObject
{
    public int questId;

    public string questName;

    [TextArea]
    public string description;

    public QuestObjectiveType objectiveType;

    [Header("Objective")]
    public int floorsAhead = 0;

    [Header("Rewards")]
    public int rewardCurrency;

    public Item rewardItem;
}
