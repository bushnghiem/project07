using System;
using UnityEngine;
using System.Collections.Generic;

public class Player : UnitBase
{
    public float linearDamping = 2f;
    public float angularDamping = 2f;

    public ClickAndFling clickAndFlingComponent;
    public ExploderComponent exploderComp;

    private Collider[] colliders;
    private Renderer[] renderers;

    public override bool IsPlayerControllable => true;
    public ProjectileDatabase projectileDatabase; // Make sure it is assigned
    public ItemDatabase itemDatabase; // Make sure it is assigned

    protected override void Awake()
    {
        base.Awake();

        exploderComp = GetComponent<ExploderComponent>();
        colliders = GetComponentsInChildren<Collider>();
        renderers = GetComponentsInChildren<Renderer>();

        rb.linearDamping = linearDamping;
        rb.angularDamping = angularDamping;
    }

    private void OnEnable()
    {
        healthComp.OnDamaged += HandleDamaged;
        healthComp.OnHealed += HandleHealed;
        healthComp.OnDeath += HandleDeath;
        clickAndFlingComponent.OnFling += HandleFling;
    }

    private void OnDisable()
    {
        healthComp.OnDamaged -= HandleDamaged;
        healthComp.OnHealed -= HandleHealed;
        healthComp.OnDeath -= HandleDeath;
        clickAndFlingComponent.OnFling -= HandleFling;
    }

    public override void Initialize(ShipRunData data)
    {
        base.Initialize(data);

        InitializeItems();

        float moveStrength = GetStat(ShipStatType.MoveStrength);
        float shotStrength = GetStat(ShipStatType.ShotStrength);

        clickAndFlingComponent.SetForces(moveStrength, shotStrength);
        RefreshItemDebug();

        SpawnEvent.OnUnitSpawned?.Invoke(this);
    }

    private void InitializeItems()
    {
        if (runData.items == null) return;

        foreach (var itemSave in runData.items)
        {
            Item item = itemDatabase.GetItem(itemSave.itemID);
            if (item != null)
                item.OnAcquire(this);
        }
    }

    public override void EquipProjectile(Projectile proj)
    {
        base.EquipProjectile(proj);
        clickAndFlingComponent.SetProjectile(proj);
    }

    public void RemoveAllPassiveEffects()
    {
        Debug.Log("Removing all effects from items");
        foreach (var item in passiveItems)
        {
            item.Remove(this);
        }
    }

    public override void Move()
    {
        clickAndFlingComponent.SetFlingable(true);
        clickAndFlingComponent.SetProjectileMode(false);
    }

    public override void Shoot()
    {
        clickAndFlingComponent.SetFlingable(true);
        clickAndFlingComponent.SetProjectileMode(true);
    }

    public override void Item()
    {
        base.Item();
        if (activeItem != null && activeItem.Use(this, this))
        {
            EndTurn();
        }
    }

    public override void StartTurn()
    {
        base.StartTurn();
        activeItem?.OnTurnStart();
        TurnEvent.OnUnitTurnStart?.Invoke(this);
    }

    public override void EndTurn()
    {
        base.EndTurn();
        clickAndFlingComponent.SetFlingable(false);
        clickAndFlingComponent.SetProjectileMode(false);
        TurnEvent.OnUnitTurnEnd?.Invoke(this);
    }

    public override void Kill()
    {
        DisablePhysics();
        DisableVisuals();

        if (exploderComp != null)
            exploderComp.StartExplosion(transform.position);

        base.Kill();
        DeathEvent.OnEntityDeath?.Invoke(this);
    }

    private void DisablePhysics()
    {
        foreach (var col in colliders)
            col.enabled = false;
    }

    private void DisableVisuals()
    {
        foreach (var r in renderers)
            r.enabled = false;
    }

    private void HandleDamaged(float damage)
    {
        Debug.Log($"Player took {damage} damage");
    }

    private void HandleHealed(float amount)
    {
        Debug.Log($"Player healed {amount}");
    }

    private void HandleDeath()
    {
        Death(); // Signal death event for effects
        Kill();
    }

    private void HandleFling(Vector3 direction, float forceStrength)
    {
        if (clickAndFlingComponent.GetProjectileMode())
        {
            Debug.Log("Shoot");
            Shot(); // Signal shoot event for effects
        }
        else
        {
            Debug.Log("Move");
            Moved();  // Signal move event for effects
        }

        EndTurn();
    }

    
}