using UnityEngine;

public class Player : MonoBehaviour, Unit
{
    public ClickAndFling clickAndFlingComponent;
    public HealthComponent healthComp;

    public Vector3 Position => transform.position;
    public bool isDead => healthComp.isDead;

    private Collider[] colliders;
    private Renderer[] renderers;

    private ShipRunData runData;

    public ActiveItem startingItem;

    private ActiveItemInstance activeItem;

    private void Awake()
    {
        healthComp = GetComponent<HealthComponent>();
        colliders = GetComponentsInChildren<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
        activeItem = new ActiveItemInstance(startingItem);
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

    public void Initialize(ShipRunData data)
    {
        runData = data;

        // Set starting HP
        Debug.Log("Spawning Player Unit with HP: " + runData.currentHealth);
        healthComp.SetMaxHealth(runData.maxHealth);
        healthComp.SetCurrentHealth(runData.currentHealth);
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
        TurnEvent.OnPlayerTurnEnd?.Invoke(this);
    }

    public void EndOfTurn()
    {
        clickAndFlingComponent.SetFlingable(false);
        clickAndFlingComponent.SetProjectileMode(false);
    }

    public void Kill()
    {
        DisablePhysics();
        DisableVisuals();
        Destroy(gameObject, 1.0f);
    }

    public void Hurt(float amount)
    {
        healthComp.Hurt(amount);
    }

    public void Heal(float amount)
    {
        healthComp.Heal(amount);
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
        DeathEvent.OnEntityDeath?.Invoke(this);
    }

    public bool GetIsDead()
    {
        return healthComp.isDead;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
