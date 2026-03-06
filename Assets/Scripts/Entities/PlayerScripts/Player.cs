using System;
using UnityEngine;
using System.Collections.Generic;

public class Player : UnitBase
{
    /*
    public event Action<UnitBase> OnStartOfTurn;
    public event Action<UnitBase> OnEndOfTurn;
    public event Action<UnitBase> OnItemUse;
    public event Action<UnitBase> OnMove;
    public event Action<UnitBase> OnShoot;
    public event Action<UnitBase> OnHeal;
    public event Action<UnitBase> OnHurt;
    public event Action<UnitBase> OnDeath;
    */

    public float linearDamping = 2f;
    public float angularDamping = 2f;

    public ClickAndFling clickAndFlingComponent;
    public ExploderComponent exploderComp;

    private Collider[] colliders;
    private Renderer[] renderers;

    public ActiveItem startingItem;
    private ActiveItemInstance activeItem;

    public List<PassiveItemInstance> passiveItems = new List<PassiveItemInstance>();

    public override bool IsPlayerControllable => true;
    public ProjectileDatabase projectileDatabase; // Make sure it is assigned
    public ActiveItemDatabase activeItemDatabase; // Make sure it is assigned
    public PassiveItemDatabase passiveItemDatabase; // Make sure it is assigned

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

        InitializeActive();
        InitializePassives();
        InitializeProjectile();

        float moveStrength = GetStat(ShipStatType.MoveStrength);
        float shotStrength = GetStat(ShipStatType.ShotStrength);

        clickAndFlingComponent.SetForces(moveStrength, shotStrength);

        SpawnEvent.OnUnitSpawned?.Invoke(this);
    }

    private void InitializeActive()
    {
        // If run already has an item, use it
        if (runData.currentActiveItem != null)
        {
            startingItem = activeItemDatabase
                .GetActiveItem(runData.currentActiveItem.activeItemID);
        }
        else
        {
            // Otherwise use template default
            string defaultItemID = template.StartingActiveItemID;

            if (string.IsNullOrEmpty(defaultItemID))
                return;

            startingItem = activeItemDatabase
                .GetActiveItem(defaultItemID);

            // Save into run data so it persists
            if (startingItem != null)
            {
                runData.currentActiveItem = new ActiveItemSaveData
                {
                    activeItemID = defaultItemID
                };
            }
        }

        if (startingItem == null)
            return;

        activeItem = new ActiveItemInstance(startingItem);
    }

    private void InitializeProjectile()
    {
        if (runData.currentProjectile == null)
            return;

        Projectile newProjectile = projectileDatabase.GetProjectile(runData.currentProjectile.projectileID);
        newProjectile.Initialize(runData.currentProjectile);
        clickAndFlingComponent.SetProjectile(newProjectile);
    }

    private void InitializePassives()
    {
        if (runData.passiveItems != null && runData.passiveItems.Count > 0)
        {
            // Load from saved run
            foreach (var passiveSave in runData.passiveItems)
            {
                PassiveItem passive = passiveItemDatabase.GetPassiveItem(passiveSave.passiveItemID);
                if (passive != null)
                    EquipPassive(passive);
            }
        }
        else
        {
            // Load from template defaults
            foreach (var passiveID in template.startingPassiveItemIDs)
            {
                PassiveItem passive = passiveItemDatabase.GetPassiveItem(passiveID);
                if (passive != null)
                {
                    EquipPassive(passive);

                    // Save to run data for persistence
                    if (runData.passiveItems == null)
                        runData.passiveItems = new List<PassiveItemSaveData>();

                    runData.passiveItems.Add(new PassiveItemSaveData
                    {
                        passiveItemID = passiveID
                    });
                }
            }
        }
    }

    public void EquipPassive(PassiveItem passive)
    {
        var instance = new PassiveItemInstance(passive);
        instance.Apply(this);
        passiveItems.Add(instance);
    }

    public void RemovePassive(PassiveItem passive)
    {
        var instance = passiveItems.Find(p => p.itemData == passive);
        if (instance != null)
        {
            instance.Remove(this);
            passiveItems.Remove(instance);
        }
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