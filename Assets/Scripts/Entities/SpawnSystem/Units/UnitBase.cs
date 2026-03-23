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

    [Header("Debug Items")]
    [SerializeField] private List<DebugStatEntry> debugStats = new();
    [SerializeField] private List<DebugItemEntry> debugItems = new();

    protected ShipRunData runData;
    protected ShipTemplate template;

    protected Dictionary<ShipStatType, float> cachedStats = new();
    protected bool statsDirty = true;

    protected Rigidbody rb;
    protected HealthComponent healthComp;
    protected DamageOnCollision collisionDamageComp;

    protected ActiveItemInstance activeItem;
    protected Projectile projectile;
    protected List<PassiveItemInstance> passiveItems = new List<PassiveItemInstance>();

    protected ItemDatabase itemDatabaseRef;

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

    public virtual void EquipPassive(PassiveItem passive)
    {
        var instance = new PassiveItemInstance(passive);
        instance.Apply(this);
        passiveItems.Add(instance);
    }

    public virtual void RemovePassive(PassiveItem passive)
    {
        var instance = passiveItems.Find(p => p.itemData == passive);
        if (instance != null)
        {
            instance.Remove(this);
            passiveItems.Remove(instance);
        }
    }

    public virtual void EquipActive(ActiveItem item)
    {
        activeItem = new ActiveItemInstance(item);
    }

    public virtual void EquipProjectile(Projectile proj)
    {
        projectile = proj;
    }

    public void AddItemToRunData(Item item)
    {
        if (runData.items == null)
            runData.items = new List<ItemSaveData>();

        if (item.slotType != ItemSlotType.Passive)
        {
            runData.items.RemoveAll(i =>
            {
                Item existing = GetItemFromID(i.itemID);
                return existing != null && existing.slotType == item.slotType;
            });
        }

        runData.items.Add(new ItemSaveData
        {
            itemID = item.itemID
        });
    }

    private Item GetItemFromID(string id)
    {
        if (itemDatabaseRef == null)
        {
            Debug.LogError("ItemDatabase reference is null!");
            return null;
        }

        return itemDatabaseRef.GetItem(id);
    }

    public void RefreshItemDebug()
    {
        debugItems.Clear();

        // Active Item
        if (activeItem != null)
        {
            debugItems.Add(new DebugItemEntry
            {
                itemType = "Active",
                itemID = activeItem.itemData.itemID,
                itemName = activeItem.itemData.itemName,
                description = activeItem.itemData.description
            });
        }

        // Projectile
        if (projectile != null)
        {
            debugItems.Add(new DebugItemEntry
            {
                itemType = "Projectile",
                itemID = projectile.ProjectileID,
                itemName = projectile.projectileName,
                description = "Projectile stats and effects"
            });
        }

        // Passive Items
        foreach (var p in passiveItems)
        {
            debugItems.Add(new DebugItemEntry
            {
                itemType = "Passive",
                itemID = p.itemData.itemID,
                itemName = p.itemData.itemName,
                description = p.itemData.description
            });
        }
    }

    public void CleanInventory()
    {
        if (runData.items == null) return;

        HashSet<ItemSlotType> occupiedSlots = new HashSet<ItemSlotType>();

        runData.items.RemoveAll(i =>
        {
            Item item = GetItemFromID(i.itemID);
            if (item == null) return true;

            if (item.slotType == ItemSlotType.Passive)
                return false;

            if (occupiedSlots.Contains(item.slotType))
                return true;

            occupiedSlots.Add(item.slotType);
            return false;
        });
    }

    public ActiveItemInstance GetActiveItem() => activeItem;
    public Projectile GetProjectile() => projectile;
    public float GetCurrentHealth() => healthComp.GetCurrentHealth();

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