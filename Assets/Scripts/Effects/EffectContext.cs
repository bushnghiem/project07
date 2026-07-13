using UnityEngine;

public struct EffectContext
{
    public Vector3 position;
    public GameObject source;
    public Entity sourceEntity;
    public UnitBase owner;

    public Vector3 direction;
    public float force;

    public ActionContext actionContext;

    public EffectContext(
        Vector3 position,
        GameObject source,
        Entity sourceEntity,
        UnitBase owner,
        ActionContext actionContext = null)
    {
        this.position = position;
        this.source = source;
        this.sourceEntity = sourceEntity;
        this.owner = owner;

        this.direction = Vector3.forward;
        this.force = 0f;

        this.actionContext = actionContext;
    }
}