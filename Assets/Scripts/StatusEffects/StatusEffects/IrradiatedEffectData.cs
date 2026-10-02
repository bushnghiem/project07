using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect/Irradiated")]
public class IrradiatedEffectData
    : StatusEffectData
{
    [Header("Damage")]
    public float damagePerStackPerTurn = 2f;

    [Header("Max Health Reduction")]
    public float maxHealthReductionPerStackPerTurn = 5f;

    public override StatusEffectInstance
        CreateInstance(Unit target)
    {
        return new IrradiatedEffectInstance();
    }
}
