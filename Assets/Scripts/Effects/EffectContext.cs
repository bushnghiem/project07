using UnityEngine;

public struct EffectContext
{
    public Vector3 position;
    public GameObject source;
    public Entity sourceEntity;

    public EffectContext(Vector3 position, GameObject source, Entity sourceEntity)
    {
        this.position = position;
        this.source = source;
        this.sourceEntity = sourceEntity;
    }
}