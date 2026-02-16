using UnityEngine;

public class Enemy : MonoBehaviour, Unit
{
    public HealthComponent healthComp;

    public Vector3 Position => transform.position;
    public bool isDead => healthComp.isDead;

    private Collider[] colliders;
    private Renderer[] renderers;

    private ShipRunData runData;

    private void Awake()
    {
        healthComp = GetComponent<HealthComponent>();
        colliders = GetComponentsInChildren<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
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
        Debug.Log("Spawning Enemy Unit with HP: " + runData.currentHealth);
        healthComp.SetMaxHealth(runData.maxHealth);
        healthComp.SetCurrentHealth(runData.currentHealth);
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
        Debug.Log($"Enemy took {damage} damage");
    }

    private void HandleHealed(float amount)
    {
        Debug.Log($"Enemy healed {amount}");
    }

    private void HandleDeath()
    {
        DeathEvent.OnEntityDeath?.Invoke(this);
    }

    public void Attack(Player target)
    {
        Debug.Log("Enemy attacks!");
        TurnEvent.OnEnemyTurnEnd?.Invoke(this);
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
