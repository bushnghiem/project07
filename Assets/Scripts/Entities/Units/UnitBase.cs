using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour, Unit
{
    public event Action<UnitBase> OnStartOfTurn;
    public event Action<UnitBase> OnEndOfTurn;
    public event Action<UnitBase> OnItemUse;
    public event Action<UnitBase> OnMove;
    public event Action<UnitBase> OnShoot;
    public event Action<UnitBase> OnHeal;
    public event Action<UnitBase> OnHurt;
    public event Action<UnitBase> OnShield;
    public event Action<UnitBase> OnDeath;

    [SerializeField] private ShipTemplateDatabase shipDatabase;
    public void SetShipDatabase(ShipTemplateDatabase db) => shipDatabase = db;

    [SerializeField] private List<DebugStatEntry> debugStats = new();

    protected ShipRunData runData;
    protected ShipTemplate template;

    protected Dictionary<ShipStatType, float> cachedStats = new();
    protected bool statsDirty = true;

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

    public float GetStat(ShipStatType statType)
    {
        if (statsDirty)
            RecalculateStats();

        return cachedStats[statType];
    }

    void RecalculateStats()
    {
        cachedStats.Clear();
        debugStats.Clear();

        foreach (ShipStatType statType in System.Enum.GetValues(typeof(ShipStatType)))
        {
            float baseValue = template.GetBaseStat(statType);

            float totalFlat = 0f;
            float totalPercent = 0f;

            foreach (var mod in runData.statModifiers)
            {
                if (mod.statType != statType)
                    continue;

                totalFlat += mod.flatBonus;
                totalPercent += mod.percentBonus;
            }

            float finalValue = (baseValue + totalFlat) * (1f + totalPercent);

            cachedStats[statType] = finalValue;

            debugStats.Add(new DebugStatEntry
            {
                statType = statType,
                value = finalValue
            });
        }

        statsDirty = false;
    }

    public void AddStatModifier(StatModifier modifier)
    {
        runData.statModifiers.Add(modifier);
        statsDirty = true;
        ApplyStats();
    }

    public void RemoveStatModifier(StatModifier modifier)
    {
        runData.statModifiers.Remove(modifier);
        statsDirty = true;
        ApplyStats();
    }

    public void RemoveModifiersFromSource(object source)
    {
        runData.statModifiers.RemoveAll(m => m.source == source);
        statsDirty = true;
        ApplyStats();
    }

    public void Hurt(float amount)
    {
        OnHurt?.Invoke(this);
        healthComp.Hurt(amount);
        runData.currentHealth = healthComp.GetCurrentHealth();
    }

    public void Heal(float amount)
    {
        OnHeal?.Invoke(this);
        healthComp.Heal(amount);
        runData.currentHealth = healthComp.GetCurrentHealth();
    }

    public void AddShield(int amount)
    {
        OnShield?.Invoke(this);
        healthComp.addShield(amount);
        Debug.Log($"{gameObject.name} gained {amount} shield");
    }

    public void Moved()
    {
        OnMove?.Invoke(this);
    }

    public void Shot()
    {
        OnShoot?.Invoke(this);
    }

    public void Death()
    {
        OnDeath?.Invoke(this);
    }

    public virtual void Kill()
    {
        runData.isDead = true;
        //Destroy(gameObject);
    }

    public abstract void Move();

    public abstract void Shoot();

    public virtual void Item()
    {
        OnItemUse?.Invoke(this);
    }

    public virtual void StartTurn()
    {
        OnStartOfTurn?.Invoke(this);
    }

    public virtual void EndTurn()
    {
        OnEndOfTurn?.Invoke(this);
    }
}