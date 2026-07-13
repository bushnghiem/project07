using UnityEngine;

[CreateAssetMenu(menuName = "Items/Active/Damage Field")]
public class DamageFieldItem : ActiveItem
{
    public GameObject fieldPrefab;

    public override ItemTargetType TargetType => ItemTargetType.Position;

    public override void Execute(
        Unit user,
        ItemTargetData data,
        ActionContext context
    )
    {
        if (fieldPrefab == null)
        {
            Debug.LogWarning("DamageFieldItem: missing prefab");
            return;
        }

        if (data.targetPosition == Vector3.zero)
            return;

        GameObject obj = Object.Instantiate(
            fieldPrefab,
            data.targetPosition,
            Quaternion.identity
        );

        // Optional: assign ownership if your field supports scaling later
        DamageField field = obj.GetComponent<DamageField>();

        if (field == null)
        {
            Debug.LogWarning("DamageField prefab missing DamageField component");
        }
    }
}