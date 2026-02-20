using UnityEngine;

public class Enemy : MonoBehaviour, Unit
{
    Rigidbody rb;
    public float linearDamping = 2.0f;
    public float angularDamping = 2.0f;

    public HealthComponent healthComp;
    public ExploderComponent exploderComp;

    public int initiative = 10;

    public Vector3 Position => transform.position;
    public bool IsPlayerControllable => false;
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

    public void Initialize(ShipRunData data)
    {
        runData = data;

        // Set starting HP
        Debug.Log("Spawning Enemy Unit with HP: " + runData.currentHealth);
        healthComp.SetMaxHealth(runData.maxHealth);
        healthComp.SetCurrentHealth(runData.currentHealth);
        //clickAndFlingComponent.SetForces(runData.moveStrength, runData.shotStrength);
        rb.mass = runData.mass;
        initiative = runData.initiative;
        startingItem = runData.currentItem;
        activeItem = new ActiveItemInstance(startingItem);
        SpawnEvent.OnUnitSpawned?.Invoke(this);
        //Debug.Log("Enemy Spawned");
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
        Debug.Log("Enemy Moved");
    }

    public void Shoot()
    {
        Debug.Log("Enemy Shot");
    }

    public void Item()
    {
        Debug.Log("Enemy Used Item");
        if (activeItem.Use(this, this))
        {
            EndTurn();
        }
        else
        {
            Debug.Log("Item still on cooldown for " + activeItem.GetRemainingCooldown() + " turns");
        }
    }

    public void Kill()
    {
        DisablePhysics();
        DisableVisuals();
        Destroy(gameObject, 1.0f);
    }

    public void StartTurn()
    {
        activeItem.OnTurnStart();
        Debug.Log("Enemy Thinking...");
        TurnEvent.OnUnitTurnStart?.Invoke(this);
        Attack();
    }

    public void EndTurn()
    {
        TurnEvent.OnUnitTurnEnd?.Invoke(this);
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
        Debug.Log($"Enemy took {damage} damage");
    }

    private void HandleHealed(float amount)
    {
        Debug.Log($"Enemy healed {amount}");
    }

    private void HandleDeath()
    {
        Kill();
        if (exploderComp != null)
        {
            exploderComp.StartExplosion(transform.position);
        }
        DeathEvent.OnEntityDeath?.Invoke(this);
    }

    public void Attack()
    {
        Debug.Log("Enemy attacks!");
        //TurnEvent.OnEnemyTurnEnd?.Invoke(this);
        EndTurn();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //SpawnEvent.OnUnitSpawned?.Invoke(this);
        //Debug.Log("Enemy Spawned");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
