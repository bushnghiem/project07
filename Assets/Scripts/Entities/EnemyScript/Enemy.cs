using UnityEngine;
using System.Collections.Generic;

public class Enemy : UnitBase
{
    public float linearDamping = 2f;
    public float angularDamping = 2f;
    public int orbitSide = 1;

    public EffectController effectController;
    public ClickAndFling clickAndFlingComp;
    public EnemyAIBase aiComp;

    private Collider[] colliders;
    private Renderer[] renderers;

    public override bool IsPlayerControllable => false;

    public ProjectileDatabase projectileDatabase;
    public ItemDatabase itemDatabase; // Assign it

    protected override void Awake()
    {
        base.Awake();

        effectController = GetComponent<EffectController>();
        clickAndFlingComp = GetComponent<ClickAndFling>();
        colliders = GetComponentsInChildren<Collider>();
        renderers = GetComponentsInChildren<Renderer>();

        rb.linearDamping = linearDamping;
        rb.angularDamping = angularDamping;
        orbitSide = Random.value > 0.5f ? 1 : -1;
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
        base.Initialize(data);

        SpawnEvent.OnUnitSpawned?.Invoke(this);

        float moveStrength = GetStat(ShipStatType.MoveStrength);
        float shotStrength = GetStat(ShipStatType.ShotStrength);

        clickAndFlingComp.SetForces(moveStrength, shotStrength);
        InitializeItems();
        RefreshItemDebug();
    }

    // Initialize all items from run data
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
        clickAndFlingComp.SetProjectile(proj);
    }

    public override void Move()
    {
        Moved(); // Signal move event for effects
        Debug.Log("Enemy Moved");
    }

    public override void Shoot()
    {
        Shot(); // Signal shoot event for effects
        Debug.Log("Enemy Shot");
    }

    public override void Item()
    {
        if (activeItem != null)
            activeItem.Use(this, this); // Use on self for enemies

        Debug.Log("Enemy Used Item");
        EndTurn();
    }

    public override void StartTurn()
    {
        base.StartTurn();
        TurnEvent.OnUnitTurnStart?.Invoke(this);
        aiComp?.TakeTurn(this);
    }

    public override void EndTurn()
    {
        base.EndTurn();
        TurnEvent.OnUnitTurnEnd?.Invoke(this);
    }

    public override void Kill()
    {
        DisablePhysics();
        DisableVisuals();

        if (effectController != null)
            effectController.TriggerEffects(transform.position, this);

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
        Debug.Log($"Enemy took {damage} damage");
    }

    private void HandleHealed(float amount)
    {
        Debug.Log($"Enemy healed {amount}");
    }

    private void HandleDeath()
    {
        Death();  // Signal death event for effects
        Kill();
    }

    private void Attack()
    {
        Debug.Log("Enemy attacks!");
        EndTurn();
    }
}