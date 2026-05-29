using UnityEngine;

[CreateAssetMenu(menuName = "Items/Active/SelfDestruct")]
public class SelfDestructionItem : ActiveItem
{
    public Effect explosion;

    public override ItemTargetType TargetType =>
        ItemTargetType.Self;

    public override void Execute(
        Unit user,
        ItemTargetData data
    )
    {
        EffectContext context = new EffectContext(
            user.Position,
            user.GameObject,
            user,
            (UnitBase)user
        );

        explosion.Execute(context);
    }
}