using UnityEngine;

public struct EffectContext
{
    public Vector3 position;
    public GameObject source;
    public Entity sourceEntity;
    public UnitBase owner;

    public Vector3 direction;
    public float force;

    public EffectContext(
        Vector3 position,
        GameObject source,
        Entity sourceEntity,
        UnitBase owner)
    {
        this.position = position;
        this.source = source;
        this.sourceEntity = sourceEntity;
        this.owner = owner;

        this.direction = Vector3.forward;
        this.force = 0f;
    }
}