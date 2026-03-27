using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Damage Field")]
public class DamageFieldEffect : Effect
{
    public GameObject fieldPrefab;

    public override void Execute(EffectContext context)
    {
        if (fieldPrefab != null)
            Object.Instantiate(fieldPrefab, context.position, Quaternion.identity);
    }
}