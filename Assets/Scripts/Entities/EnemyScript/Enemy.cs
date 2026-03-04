using UnityEngine;

public class Enemy : UnitBase
{
    public float linearDamping = 2f;
    public float angularDamping = 2f;

    public ExploderComponent exploderComp;

    private Collider[] colliders;
    private Renderer[] renderers;

    public override bool IsPlayerControllable => false;

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
    }

    public override void Move()
    {
        Debug.Log("Enemy Moved");
    }

    public override void Shoot()
    {
        Debug.Log("Enemy Shot");
    }

    public override void Item()
    {
        Debug.Log("Enemy Used Item");
        EndTurn();
    }

    public override void StartTurn()
    {
        TurnEvent.OnUnitTurnStart?.Invoke(this);
        Attack();
    }

    public override void EndTurn()
    {
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
        Debug.Log($"Enemy took {damage} damage");
    }

    private void HandleHealed(float amount)
    {
        Debug.Log($"Enemy healed {amount}");
    }

    private void HandleDeath()
    {
        Kill();
    }

    private void Attack()
    {
        Debug.Log("Enemy attacks!");
        EndTurn();
    }
}