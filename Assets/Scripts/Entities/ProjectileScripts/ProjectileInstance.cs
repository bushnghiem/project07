using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ProjectileInstance : MonoBehaviour, Entity, IInspectable
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
    private bool deathProcessed;

    private Collider[] colliders;
    private Renderer[] renderers;

    private ProjectileVisualController visualController;
    private SphereCollider sphereCollider;

    private ProjectileAudioController audioController;

    private AttackContext attackContext;
    public AttackContext AttackContext => attackContext;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthComp = GetComponent<HealthComponent>();
        effectController = GetComponent<EffectController>();
        collisionDamageComp = GetComponent<DamageOnCollision>();
        colliders = GetComponentsInChildren<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
        visualController = GetComponent<ProjectileVisualController>();
        sphereCollider = GetComponent<SphereCollider>();
        audioController = GetComponent<ProjectileAudioController>();
    }

    public void Initialize(
        Projectile stats,
        UnitBase owner,
        AttackContext attack)
    {
        this.template = stats;
        this.owner = owner;
        this.attackContext = attack;

        rb = rb ?? GetComponent<Rigidbody>();
        healthComp = healthComp ?? GetComponent<HealthComponent>();
        collisionDamageComp = collisionDamageComp ?? GetComponent<DamageOnCollision>();

        ApplyStats();
        ApplyEffects();

        visualController?.ApplyVisuals(template.VisualData);
        audioController?.Initialize(template.AudioData);

        audioController?.PlayLaunch();

        attackContext?.RegisterProjectile(this);
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
            Mathf.RoundToInt(template.GetBaseStat(ProjectileStatType.StartingShield)
            + owner.GetStat(ShipStatType.ProjectileShield)));

        float damage =
            (template.GetBaseStat(ProjectileStatType.CollisionDamage)
            + owner.GetStat(ShipStatType.ProjectileDamage));

        float knockback =
            (template.GetBaseStat(ProjectileStatType.CollisionKnockback)
            + owner.GetStat(ShipStatType.ProjectileKnockback));

        collisionDamageComp.SetCollisionStats(damage, knockback);

        if (owner != null)
        {
            foreach (var applied in owner.ProjectileCollisionStatusModifiers)
            {
                if (applied.effect == null)
                    continue;

                collisionDamageComp.statusEffects.Add(applied);
            }
        }

        if (template.useLifetime)
        {
            Invoke(nameof(Expire), template.lifeTime);
        }

        if (sphereCollider != null)
        {
            sphereCollider.radius = template.collisionRadius;
        }
    }

    void ApplyEffects()
    {
        if (effectController == null) return;

        if (template.effects != null)
        {
            foreach (var effect in template.effects)
            {
                if (effect == null)
                    continue;

                effectController.effects.Add(
                    Instantiate(effect)
                );
            }
        }

        if (owner != null)
        {
            foreach (var effect in owner.ProjectileEffectModifiers)
            {
                if (effect == null)
                    continue;
                Effect projectileRuntimeEffect = Instantiate(effect);

                effectController.effects.Add(projectileRuntimeEffect);
            }
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
        visualController?.HideVisuals();
    }

    public void Kill()
    {
        DisablePhysics();
        DisableVisuals();
        Destroy(gameObject, 1f);
    }

    public void Hurt(DamageInfo damageInfo)
    {
        healthComp.Hurt(damageInfo);

        if (effectController != null)
        {
            EffectContext context = new EffectContext(
                transform.position,
                gameObject,
                this,
                owner,
                attackContext
            );

            effectController.TriggerEffects(
                EffectTrigger.OnHit,
                context);
        }
    }

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
        if (deathProcessed) return;

        deathProcessed = true;

        CancelInvoke();

        if (effectController != null)
        {
            EffectContext context = new EffectContext(
                transform.position,
                gameObject,
                this,
                owner,
                attackContext
            );

            effectController.TriggerEffects(
                EffectTrigger.OnDeath,
                context);
        }

        attackContext?.UnregisterProjectile(this);

        Kill();
        DeathEvent.OnEntityDeath?.Invoke(this);
    }

    void FixedUpdate()
    {
        if (isDead) return;

        if (rb.linearVelocity.sqrMagnitude < template.velocityThreshold * template.velocityThreshold)
        {
            stopTimer += Time.fixedDeltaTime;
            if (stopTimer >= template.stopTimeRequired)
            {
                ProjectileEvent.OnProjectileStopped?.Invoke(this);
                attackContext?.ProjectileStopped(this);
                if (template.dieWhenStopped)
                {
                    Expire();
                }
            }
                
        }
        else stopTimer = 0f;
    }

    public void Fling(Vector3 direction, float force)
    {
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
    }

    public InspectionData GetInspectionData()
    {
        return new InspectionData
        {
            Name = template.projectileName,

            CurrentHP = healthComp.GetCurrentHealth(),
            MaxHP = healthComp.GetMaxHealth(),

            Shield = healthComp.GetShield(),

            CollisionDamage = collisionDamageComp.ContactDamage,

            ExtraInfo = "Projectile"
        };
    }
}