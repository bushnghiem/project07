using UnityEngine;

public class AIItemEvaluation
{
    public float score;
    public ItemTargetData targetData;

    public AIItemEvaluation(
        float score,
        ItemTargetData targetData)
    {
        this.score = score;
        this.targetData = targetData;
    }
}
