using UnityEngine;

public class Player : MonoBehaviour, Unit
{
    Rigidbody rb;
    public ClickAndFling clickAndFlingComponent;
    public HealthComponent healthComp;

    public int initiative = 10;

    public Vector3 Position => transform.position;
    public bool IsPlayerControllable => true;
    public int Initiative => initiative;
    public bool isDead => healthComp.isDead;

    private Collider[] colliders;
    private Renderer[] renderers;

    private ShipRunData runData;

    public ActiveItem startingItem;

    private ActiveItemInstance activeItem;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthComp = GetComponent<HealthComponent>();
        colliders = GetComponentsInChildren<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
        activeItem = new ActiveItemInstance(startingItem);

        if (healthComp == null)
            Debug.LogError("Missing HealthComponent!");

        if (clickAndFlingComponent == null)
            Debug.LogError("Missing ClickAndFling!");
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

    public void Initialize(ShipRunData data)
    {
        runData = data;

        // Set starting stats
        Debug.Log("Spawning Player Unit with HP: " + runData.currentHealth);
        healthComp.SetMaxHealth(runData.maxHealth);
        healthComp.SetCurrentHealth(runData.currentHealth);
        clickAndFlingComponent.SetForces(runData.moveStrength, runData.shotStrength);
        rb.mass = runData.mass;
        initiative = runData.initiative;
        SpawnEvent.OnUnitSpawned?.Invoke(this);
        //Debug.Log("Player Spawned");
    }

    void DisablePhysics()
    {
        foreach (var col in colliders)
            col.enabled = false;
    }

    void DisableVisuals()
    {
        foreach (var r in renderers)
            r.enabled = false;
    }

    public void Move()
    {
        clickAndFlingComponent.SetFlingable(true);
        clickAndFlingComponent.SetProjectileMode(false);
    }

    public void Shoot()
    {
        clickAndFlingComponent.SetFlingable(true);
        clickAndFlingComponent.SetProjectileMode(true);
    }

    public void Item()
    {
        activeItem.Use(this, this);
        EndTurn();
    }

    public void Kill()
    {
        DisablePhysics();
        DisableVisuals();
        Destroy(gameObject, 1.0f);
    }

    public void StartTurn()
    {
        TurnEvent.OnUnitTurnStart?.Invoke(this);
    }

    public void EndTurn()
    {
        clickAndFlingComponent.SetFlingable(false);
        clickAndFlingComponent.SetProjectileMode(false);
        //TurnEvent.OnPlayerTurnEnd?.Invoke(this);
        TurnEvent.OnUnitTurnEnd?.Invoke(this);
    }

    public void EndOfTurn()
    {

    }

    public void Hurt(float amount)
    {
        healthComp.Hurt(amount);
    }

    public void Heal(float amount)
    {
        healthComp.Heal(amount);
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
        DeathEvent.OnEntityDeath?.Invoke(this);
    }

    private void HandleFling(Vector3 direction, float forceStrength)
    {
        Debug.Log("Fling in " +  direction + " at this this strength " + forceStrength);
        EndTurn();
    }

    public bool GetIsDead()
    {
        return healthComp.isDead;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //SpawnEvent.OnUnitSpawned?.Invoke(this);
        //Debug.Log("Player Spawned");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
