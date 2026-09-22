using UnityEngine;

public class AIItemCandidate
{
    public ActiveItemInstance item;
    public ItemTargetData targetData;

    public float score;

    public AIItemCandidate(
        ActiveItemInstance item,
        ItemTargetData targetData,
        float score)
    {
        this.item = item;
        this.targetData = targetData;
        this.score = score;
    }
}

