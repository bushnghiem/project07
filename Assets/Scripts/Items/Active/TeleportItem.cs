using UnityEngine;

[CreateAssetMenu(menuName = "Items/Active/Teleport")]
public class TeleportItem : ActiveItem
{
    public override ItemTargetType TargetType =>
        ItemTargetType.Position;

    public override void Execute(
        Unit user,
        ItemTargetData data,
        ActionContext context
    )
    {
        if (user is not UnitBase unit)
            return;

        if (!unit.TeleportTo(data.targetPosition))
        {
            Debug.Log(
                $"Teleport failed for {unit.gameObject.name}"
            );

            return;
        }

        Debug.Log(
            $"{unit.gameObject.name} teleported to {data.targetPosition}"
        );
    }

    public override bool IsValidTarget(
    UnitBase user,
    ItemTargetData target
    )
    {
        if (user == null)
            return false;

        return user.CanTeleportTo(
            target.targetPosition
        );
    }

}
