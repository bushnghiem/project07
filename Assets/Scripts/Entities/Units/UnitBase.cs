using UnityEngine;

public abstract class UnitBase : MonoBehaviour, Unit
{
    [SerializeField] private ShipTemplateDatabase shipDatabase;
    public void SetShipDatabase(ShipTemplateDatabase db) => shipDatabase = db;

    protected ShipRunData runData;
    protected ShipTemplate template;

    protected Rigidbody rb;
    protected HealthComponent healthComp;
    protected DamageOnCollision collisionDamageComp;

    public abstract bool IsPlayerControllable { get; }

    public virtual int Initiative =>
        Mathf.RoundToInt(GetStat(ShipStatType.Initiative));

    public Vector3 Position => transform.position;
    public bool isDead => healthComp != null && healthComp.isDead;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthComp = GetComponent<HealthComponent>();
        collisionDamageComp = GetComponent<DamageOnCollision>();
    }

    public virtual void Initialize(ShipRunData data)
    {
        runData = data;
        template = shipDatabase.GetTemplate(runData.templateID);

        ApplyStats();
    }

    protected virtual void ApplyStats()
    {
        float maxHealth = GetStat(ShipStatType.MaxHealth);
        int shield = Mathf.RoundToInt(GetStat(ShipStatType.StartingShield));
        float mass = GetStat(ShipStatType.Mass);
        float collisionDamage = GetStat(ShipStatType.CollisionDamage);
        float collisionKnockback = GetStat(ShipStatType.CollisionKnockback);

        healthComp.SetMaxHealth(maxHealth);
        healthComp.SetShield(shield);

        healthComp.SetCurrentHealth(
            runData.currentHealth > 0 ? runData.currentHealth : maxHealth
        );

        rb.mass = mass;

        if (collisionDamageComp != null)
            collisionDamageComp.SetCollisionStats(collisionDamage, collisionKnockback);
    }

    protected float GetStat(ShipStatType statType)
    {
        float baseValue = template.GetBaseStat(statType);

        foreach (var mod in runData.statModifiers)
        {
            if (mod.statType == statType)
                baseValue = mod.Apply(baseValue);
        }

        return baseValue;
    }

    public void Hurt(float amount)
    {
        healthComp.Hurt(amount);
        runData.currentHealth = healthComp.GetCurrentHealth();
    }

    public void Heal(float amount)
    {
        healthComp.Heal(amount);
        runData.currentHealth = healthComp.GetCurrentHealth();
    }

    public void AddShield(int amount)
    {
        healthComp.addShield(amount);
        Debug.Log($"{gameObject.name} gained {amount} shield");
    }

    public virtual void Kill()
    {
        runData.isDead = true;
        //Destroy(gameObject);
    }

    public abstract void Move();
    public abstract void Shoot();
    public abstract void Item();
    public abstract void StartTurn();
    public abstract void EndTurn();
}