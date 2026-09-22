using UnityEngine;

[CreateAssetMenu(menuName = "Items/Active/Explosion")]
public class ExplosionItem : ActiveItem
{
    public Effect explosion;

    public override ItemTargetType TargetType =>
        ItemTargetType.Unit;

    public override void Execute(
        Unit user,
        ItemTargetData data,
        ActionContext context)
    {
        if (data == null || data.targetUnit == null)
            return;

        EffectContext effectContext = new EffectContext(
            data.targetUnit.Position,
            data.targetUnit.GameObject,
            user,
            (UnitBase)user
        );

        explosion.Execute(effectContext);
    }
}
