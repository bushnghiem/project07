using UnityEngine;

public class Player : UnitBase
{
    public float linearDamping = 2f;
    public float angularDamping = 2f;

    public ClickAndFling clickAndFlingComponent;
    public ExploderComponent exploderComp;

    private Collider[] colliders;
    private Renderer[] renderers;

    public ActiveItem startingItem;
    private ActiveItemInstance activeItem;

    public override bool IsPlayerControllable => true;
    public ProjectileDatabase projectileDatabase; // Make sure it is assigned

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

        float moveStrength = GetStat(ShipStatType.MoveStrength);
        float shotStrength = GetStat(ShipStatType.ShotStrength);

        clickAndFlingComponent.SetForces(moveStrength, shotStrength);

        InitializeItems();
        InitializeProjectile();

        SpawnEvent.OnUnitSpawned?.Invoke(this);
    }

    private void InitializeItems()
    {
        if (runData.currentActiveItem == null)
            return;

        startingItem = ActiveItemDatabase.Instance
            .GetActiveItem(runData.currentActiveItem.activeItemID);

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
        if (activeItem != null && activeItem.Use(this, this))
            EndTurn();
    }

    public override void StartTurn()
    {
        activeItem?.OnTurnStart();
        TurnEvent.OnUnitTurnStart?.Invoke(this);
    }

    public override void EndTurn()
    {
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
        Kill();
    }

    private void HandleFling(Vector3 direction, float forceStrength)
    {
        EndTurn();
    }
}