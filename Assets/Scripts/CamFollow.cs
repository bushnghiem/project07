using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Entity target;

    public Vector3 offset;
    public float smoothSpeed = 0.125f;


    private void OnEnable()
    {
        TurnEvent.OnUnitTurnStart += HandleUnitTurnStart;
        DeathEvent.OnEntityDeath += HandleDeath;
        ProjectileSpawnEvent.AddCamFollow += HandleCamFollow;
    }

    private void OnDisable()
    {
        TurnEvent.OnUnitTurnStart -= HandleUnitTurnStart;
        DeathEvent.OnEntityDeath -= HandleDeath;
        ProjectileSpawnEvent.AddCamFollow -= HandleCamFollow;
    }

    public void HandleUnitTurnStart(Unit unit)
    {
        if (unit != null)
        {
            Debug.Log("Swapped to " + unit);
            target = unit;
        }
    }

    public void HandleDeath(Entity entity)
    {
        if (entity == target)
        {
            //Debug.Log(entity + " died");
            target = null;
        }
    }

    public void HandleCamFollow(Entity entity)
    {
        if (entity != null)
        {
            Debug.Log("Swapped to " + entity);
            target = entity;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.Position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        }
    }
}