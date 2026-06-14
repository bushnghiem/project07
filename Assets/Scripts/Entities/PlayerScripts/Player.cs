using System;
using UnityEngine;
using System.Collections.Generic;

public class Player : UnitBase
{
    public float linearDamping = 2f;
    public float angularDamping = 2f;

    public ClickAndFling clickAndFlingComponent;

    private Collider[] colliders;
    private Renderer[] renderers;

    public override bool IsPlayerControllable => true;
    public ProjectileDatabase projectileDatabase; // Make sure it is assigned
    public ItemDatabase itemDatabase; // Make sure it is assigned

    private UnitActionExecutor executor;

    protected override void Awake()
    {
        base.Awake();

        colliders = GetComponentsInChildren<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
        executor = FindFirstObjectByType<UnitActionExecutor>();

        rb.linearDamping = linearDamping;
        rb.angularDamping = angularDamping;
    }

    private void OnEnable()
    {
        healthComp.OnDamaged += HandleDamaged;
        healthComp.OnHealed += HandleHealed;
        healthComp.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        healthComp.OnDamaged -= HandleDamaged;
        healthComp.OnHealed -= HandleHealed;
        healthComp.OnDeath -= HandleDeath;
    }

    public override void Initialize(ShipRunData data)
    {
        itemDatabaseRef = itemDatabase;
        base.Initialize(data);

        ClearAllStatModifiers();
        passiveItems.Clear();

        projectileEffectModifiers.Clear();
        onShootEffectsFromProjectile.Clear();

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

        CleanInventory();

        foreach (var itemSave in runData.items)
        {
            Item item = itemDatabase.GetItem(itemSave.itemID);
            if (item != null)
            {
                item.OnAcquire(this);
            }
        }
    }

    public override void EquipProjectile(ProjectileItem proj)
    {
        base.EquipProjectile(proj);
        clickAndFlingComponent.SetProjectile(proj.projectile);
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

        clickAndFlingComponent.SetActionType(
            ActionType.Move
        );
    }

    public override void Moved()
    {
        base.Moved();
        clickAndFlingComponent.SetFlingable(false);
    }

    public override void Shoot()
    {
        clickAndFlingComponent.SetFlingable(true);

        clickAndFlingComponent.SetActionType(
            ActionType.Shoot
        );
    }

    public override void Item()
    {
        EnsureExecutor();

        if (executor == null)
        {
            Debug.LogError("No UnitActionExecutor found in scene.");
            return;
        }

        UnitAction action = new UnitAction
        {
            actor = this,
            actionType = ActionType.Item,
            activeItem = activeItem
        };

        executor.Execute(action);
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
        clickAndFlingComponent.SetActionType(ActionType.Move);

        TurnEvent.OnUnitTurnEnd?.Invoke(this);
    }

    public override void Kill()
    {
        DisablePhysics();
        DisableVisuals();

        if (effectController != null)
            effectController.TriggerEffects(EffectTrigger.OnDeath, transform.position, this);

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
        GetComponent<ShipVisualController>()?.OnDeath();
        foreach (var r in renderers)
            r.enabled = false;
    }

    public override void Hurt(DamageInfo damageInfo)
    {
        base.Hurt(damageInfo);
        runData.currentHealth = healthComp.GetCurrentHealth(); // Make damage track throughout scenes and save
    }

    public override void Heal(float amount)
    {
        base.Heal(amount);
        runData.currentHealth = healthComp.GetCurrentHealth(); // Make healing track throughout scenes and save
    }

    private void HandleDamaged(float damage)
    {
        //Debug.Log($"Player took {damage} damage");
    }

    private void HandleHealed(float amount)
    {
        //Debug.Log($"Player healed {amount}");
    }

    private void HandleDeath()
    {
        Death(); // Signal death event for effects
        Kill();
    }

    private void EnsureExecutor()
    {
        if (executor == null)
            executor = FindFirstObjectByType<UnitActionExecutor>();
    }
}