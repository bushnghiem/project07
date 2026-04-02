using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ProjectileInstance : MonoBehaviour, Entity
{
    Rigidbody rb;
    float stopTimer;

    public HealthComponent healthComp;
    public EffectController effectController;
    public DamageOnCollision collisionDamageComp;

    private Projectile template;
    private UnitBase owner;

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
        effectController = GetComponent<EffectController>();
        collisionDamageComp = GetComponent<DamageOnCollision>();
        colliders = GetComponentsInChildren<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void Initialize(Projectile stats, UnitBase owner)
    {
        this.template = stats;
        this.owner = owner;

        rb = rb ?? GetComponent<Rigidbody>();
        healthComp = healthComp ?? GetComponent<HealthComponent>();
        collisionDamageComp = collisionDamageComp ?? GetComponent<DamageOnCollision>();

        ApplyStats();
    }

    private void ApplyStats()
    {
        float mass =
            template.GetBaseStat(ProjectileStatType.Mass)
            + owner.GetStat(ShipStatType.ProjectileMass);

        rb.mass = mass;
        rb.linearDamping = template.linearDamping;
        rb.angularDamping = template.angularDamping;

        float maxHealth =
            template.GetBaseStat(ProjectileStatType.MaxHealth)
            + owner.GetStat(ShipStatType.ProjectileHealth);

        healthComp.SetMaxHealth(maxHealth);
        healthComp.SetCurrentHealth(maxHealth);

        healthComp.SetShield(
            Mathf.RoundToInt(template.GetBaseStat(ProjectileStatType.StartingShield))
        );

        float damage =
            (template.GetBaseStat(ProjectileStatType.CollisionDamage)
            + owner.GetStat(ShipStatType.ProjectileDamage));

        float knockback =
            (template.GetBaseStat(ProjectileStatType.CollisionKnockback)
            + owner.GetStat(ShipStatType.ProjectileKnockback));

        collisionDamageComp.SetCollisionStats(damage, knockback);

        if (template.useLifetime)
        {
            Invoke(nameof(Expire), template.lifeTime);
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

        if ((template.doesExplode) && (effectController != null))
            effectController.TriggerEffects(transform.position);

        Kill();
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