using UnityEngine;

public class ProjectileInstance : MonoBehaviour, Entity
{
    Rigidbody rb;
    float stopTimer;

    private Projectile stats;
    public HealthComponent healthComp;
    public ExploderComponent exploderComp;
    public DamageOnCollision collisionDamageComp;

    public Vector3 Position => transform.position;
    public bool isDead => healthComp.isDead;

    private Collider[] colliders;
    private Renderer[] renderers;

    public bool dieWhenStopped = true;
    public float velocityThreshold = 0.1f; // How slow is "stopped"
    public float stopTimeRequired = 0.5f; // Must stay slow below velocityThreshold this long

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthComp = GetComponent<HealthComponent>();
        exploderComp = GetComponent<ExploderComponent>();
        collisionDamageComp = GetComponent<DamageOnCollision>();
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

    public void Initialize(Projectile newStats)
    {
        stats = newStats;

        rb.mass = stats.mass;
        rb.linearDamping = stats.linearDamping;
        rb.angularDamping = stats.angularDamping;


        healthComp.SetMaxHealth(stats.maxHealth);
        healthComp.SetCurrentHealth(stats.maxHealth);

        if (stats.useLifetime)
            Invoke(nameof(Expire), stats.lifeTime);

        velocityThreshold = stats.velocityThreshold;
        stopTimeRequired = stats.stopTimeRequired;
        dieWhenStopped = stats.dieWhenStopped;

        if (exploderComp != null)
        {
            exploderComp.SetExplosionStats(stats.explosionStats);
        }

        collisionDamageComp.SetCollisionStats(stats.collisionDamage, stats.collisionKnockback);

    }

    void Expire()
    {
        healthComp.SetCurrentHealth(0);
    }

    private void HandleDamaged(float damage)
    {
        Debug.Log($"Projectile took {damage} damage");
    }

    private void HandleHealed(float amount)
    {
        Debug.Log($"Projectile healed {amount}");
    }

    private void HandleDeath()
    {
        CancelInvoke();
        Kill();
        if (exploderComp != null)
        {
            if (stats.doesExplode)
            {
                exploderComp.StartExplosion(transform.position);
            }
        }
        DeathEvent.OnEntityDeath?.Invoke(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (isDead) return;
        if (!dieWhenStopped) return;

        if (rb.linearVelocity.sqrMagnitude < velocityThreshold * velocityThreshold)
        {
            stopTimer += Time.fixedDeltaTime;

            if (stopTimer >= stopTimeRequired)
            {
                Expire();
            }
        }
        else
        {
            stopTimer = 0f;
        }
    }


    public void Fling(Vector3 direction, float forceStrength)
    {
        rb.AddForce(direction * forceStrength, ForceMode.Impulse);
    }
}
