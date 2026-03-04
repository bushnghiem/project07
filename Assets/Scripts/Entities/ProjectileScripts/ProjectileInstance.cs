using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ProjectileInstance : MonoBehaviour, Entity
{
    Rigidbody rb;
    float stopTimer;

    public HealthComponent healthComp;
    public ExploderComponent exploderComp;
    public DamageOnCollision collisionDamageComp;

    private Projectile template;

    // Copy of stats for this instance
    private List<ProjectileBaseStatEntry> instanceStats;

    public Vector3 Position => transform.position;
    public bool isDead => healthComp.isDead;

    private Collider[] colliders;
    private Renderer[] renderers;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthComp = GetComponent<HealthComponent>();
        exploderComp = GetComponent<ExploderComponent>();
        collisionDamageComp = GetComponent<DamageOnCollision>();
        colliders = GetComponentsInChildren<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void Initialize(Projectile stats, ProjectileSaveData saveData = null)
    {
        template = stats;

        // Clone the base stats so the scriptable object isn't changed
        instanceStats = template.baseStats
            .Select(e => new ProjectileBaseStatEntry { statType = e.statType, value = e.value })
            .ToList();

        // Apply save data modifiers if provided
        if (saveData != null)
        {
            foreach (var modifier in saveData.statModifiers)
            {
                var stat = instanceStats.Find(s => s.statType == modifier.statType);
                if (stat != null)
                    stat.value = modifier.Apply(stat.value);
            }
        }

        rb = rb ?? GetComponent<Rigidbody>();
        rb.mass = GetStat(ProjectileStatType.Mass);
        rb.linearDamping = template.linearDamping;
        rb.angularDamping = template.angularDamping;

        healthComp = healthComp ?? GetComponent<HealthComponent>();
        float maxHealth = GetStat(ProjectileStatType.MaxHealth);
        healthComp.SetMaxHealth(maxHealth);
        healthComp.SetCurrentHealth(maxHealth);
        healthComp.SetShield(Mathf.RoundToInt(GetStat(ProjectileStatType.StartingShield)));

        collisionDamageComp = collisionDamageComp ?? GetComponent<DamageOnCollision>();
        collisionDamageComp.SetCollisionStats(
            GetStat(ProjectileStatType.CollisionDamage),
            GetStat(ProjectileStatType.CollisionKnockback)
        );

        exploderComp = exploderComp ?? GetComponent<ExploderComponent>();
        if (exploderComp != null)
        {
            var explosion = template.explosionStats;
            if (saveData != null)
            {
                explosion = new ExplosionStats
                {
                    radius = explosion.radius + saveData.bonusExplosionStats.radius,
                    damage = explosion.damage + saveData.bonusExplosionStats.damage,
                    force = explosion.force + saveData.bonusExplosionStats.force,
                    damageLayers = saveData.bonusExplosionStats.damageLayers
                };
            }
            exploderComp.SetExplosionStats(explosion);
        }

        colliders = colliders ?? GetComponentsInChildren<Collider>();
        renderers = renderers ?? GetComponentsInChildren<Renderer>();

        bool useLifetime = saveData != null ? saveData.useLifetimeOverride : template.useLifetime;
        if (useLifetime)
        {
            float lifetime = template.lifeTime;
            Invoke(nameof(Expire), lifetime);
        }
    }

    // Get stat value from instance copy
    private float GetStat(ProjectileStatType type)
    {
        var entry = instanceStats.Find(s => s.statType == type);
        return entry != null ? entry.value : 0f;
    }

    void Expire() => healthComp.SetCurrentHealth(0);

    void DisablePhysics()
    {
        foreach (var col in colliders) col.enabled = false;
    }

    void DisableVisuals()
    {
        foreach (var r in renderers) r.enabled = false;
    }

    public void Kill()
    {
        DisablePhysics();
        DisableVisuals();
        Destroy(gameObject, 1f);
    }

    public void Hurt(float amount) => healthComp.Hurt(amount);
    public void Heal(float amount) => healthComp.Heal(amount);

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

    private void HandleDamaged(float dmg) => Debug.Log($"{gameObject.name} took {dmg} damage");
    private void HandleHealed(float amt) => Debug.Log($"{gameObject.name} healed {amt}");

    private void HandleDeath()
    {
        CancelInvoke();
        Kill();
        if ((template.doesExplode) && (exploderComp != null))
            exploderComp.StartExplosion(transform.position);

        DeathEvent.OnEntityDeath?.Invoke(this);
    }

    void FixedUpdate()
    {
        if (!template.dieWhenStopped || isDead) return;

        if (rb.linearVelocity.sqrMagnitude < template.velocityThreshold * template.velocityThreshold)
        {
            stopTimer += Time.fixedDeltaTime;
            if (stopTimer >= template.stopTimeRequired) Expire();
        }
        else stopTimer = 0f;
    }

    public void Fling(Vector3 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode.Impulse);
    }
}