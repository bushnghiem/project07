using UnityEngine;

[CreateAssetMenu(menuName = "Items/Active/SelfDestruct")]
public class SelfDestructionItem : ActiveItem
{
    public ExplosionStats stats;

    public override void Activate(Unit user, Unit target)
    {
        if (target == null) return;

        ExplodeEvent.OnExplode(stats, user.Position);
    }
}
