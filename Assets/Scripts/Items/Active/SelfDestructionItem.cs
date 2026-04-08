using UnityEngine;

[CreateAssetMenu(menuName = "Items/Active/SelfDestruct")]
public class SelfDestructionItem : ActiveItem
{
    public Effect explosion;

    public override void Activate(Unit user, Unit target)
    {
        if (target == null) return;

        EffectContext context = new EffectContext(
            user.Position,
            user.GameObject,
            user,
            (UnitBase)user
        );

        explosion.Execute(context);
    }
}
