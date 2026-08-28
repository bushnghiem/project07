using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect/Moving Ram")]
public class MovingRamEffectData : StatusEffectData
{
    public float collisionDamageBonus = 0.50f;

    public override StatusEffectInstance CreateInstance(Unit target)
    {
        return new MovingRamEffectInstance();
    }
}
