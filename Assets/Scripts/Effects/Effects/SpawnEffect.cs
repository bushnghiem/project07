using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Spawn")]
public class SpawnEffect : Effect
{
    public GameObject prefab;
    public int count = 1;
    public float spread = 1f;

    public override void Execute(EffectContext context)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 offset = Random.insideUnitSphere * spread;
            offset.y = 0f;

            Object.Instantiate(prefab, context.position + offset, Random.rotation);
        }
    }
}