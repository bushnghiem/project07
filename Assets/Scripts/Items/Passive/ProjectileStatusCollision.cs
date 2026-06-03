using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Items/Passive/Projectile Status")]
public class ProjectileStatusCollision : PassiveItem
{
    public List<StatusEffectData> statusEffects = new();

    public override void ApplyEffect(
        Unit unit,
        PassiveItemInstance instance)
    {
        if (unit is not UnitBase unitBase)
            return;

        foreach (var effect in statusEffects)
        {
            unitBase.AddProjectileCollisionStatus(effect);
        }
    }

    public override void RemoveEffect(
        Unit unit,
        PassiveItemInstance instance)
    {
        if (unit is not UnitBase unitBase)
            return;

        foreach (var effect in statusEffects)
        {
            unitBase.RemoveProjectileCollisionStatus(effect);
        }
    }

    public override void ApplyEffect(Unit unit) { }
}
