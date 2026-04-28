using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour, Unit
{
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
    protected EffectController effectController;
    protected StatusEffectController statusController;

    protected ActiveItemInstance activeItem;
    protected Projectile projectile;
    protected List<PassiveItemInstance> passiveItems = new List<PassiveItemInstance>();

    protected List<Effect> projectileInjectedEffects = new();

    protected ItemDatabase itemDatabaseRef;

    public GameObject GameObject => gameObject;

    public abstract bool IsPlayerControllable { get; }

    public virtual int Initiative =>
        Mathf.RoundToInt(GetStat(ShipStatType.Initiative));

    public Vector3 Position => transform.position;
    public bool isDead => healthComp != null && healthComp.isDead;

    protected int currentAP;
    public int CurrentAP => currentAP;
    protected int MaxAP;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthComp = GetComponent<HealthComponent>();
        collisionDamageComp = GetComponent<DamageOnCollision>();
        effectController = GetComponent<EffectController>();
        statusController = GetComponent<StatusEffectController>();
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
        MaxAP = (int)GetStat(ShipStatType.ActionPoints);

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

    public void RemoveModifiersFromSource(string sourceID)
    {
        runData.statModifiers.RemoveAll(m => m.sourceID == sourceID);
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

        if (effectController == null) return;

        RemoveProjectileEffects(effectController);

        if (proj.effects != null)
        {
            foreach (var effect in proj.effects)
            {
                if (effect.trigger == EffectTrigger.OnShoot)
                {
                    effectController.effects.Add(effect);
                    projectileInjectedEffects.Add(effect);
                }
            }
        }
    }

    protected void RemoveProjectileEffects(EffectController effectController)
    {
        foreach (var effect in projectileInjectedEffects)
        {
            effectController.effects.Remove(effect);
        }

        projectileInjectedEffects.Clear();
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

    public void ClearAllStatModifiers()
    {
        if (runData.statModifiers == null)
            runData.statModifiers = new List<StatModifier>();

        runData.statModifiers.Clear();
        statsDirty = true;
    }

    public virtual bool TriggerShootEffects(Vector3 direction, float force)
    {
        if (effectController == null) return false;

        bool hasShootEffect = false;

        foreach (var effect in effectController.effects)
        {
            if (effect.trigger == EffectTrigger.OnShoot)
            {
                hasShootEffect = true;
                break;
            }
        }

        if (!hasShootEffect) return false;

        var context = new EffectContext(
            transform.position,
            gameObject,
            this,
            this
        );

        context.direction = direction;
        context.force = force;

        effectController.TriggerEffects(EffectTrigger.OnShoot, context);

        return true;
    }

    public ActiveItemInstance GetActiveItem() => activeItem;
    public Projectile GetProjectile() => projectile;
    public float GetCurrentHealth() => healthComp.GetCurrentHealth();

    public virtual void Hurt(float amount)
    {
        healthComp.Hurt(amount);

        EventBus.Raise(new UnitEvent
        {
            source = this,
            target = this,
            type = UnitEventType.Hurt,
            value = amount
        });
    }

    public virtual void Heal(float amount)
    {
        healthComp.Heal(amount);

        EventBus.Raise(new UnitEvent
        {
            source = this,
            target = this,
            type = UnitEventType.Heal,
            value = amount
        });
    }

    public void AddShield(int amount)
    {
        healthComp.addShield(amount);
        EventBus.Raise(new UnitEvent
        {
            source = this,
            type = UnitEventType.Shield,
            value = amount
        });
        Debug.Log($"{gameObject.name} gained {amount} shield");
    }

    public void Moved()
    {
        EventBus.Raise(new UnitEvent
        {
            source = this,
            type = UnitEventType.Move
        });
    }

    public void Shot()
    {
        EventBus.Raise(new UnitEvent
        {
            source = this,
            type = UnitEventType.Shoot
        });
    }

    public void Death()
    {
        EventBus.Raise(new UnitEvent
        {
            source = this,
            target = this,
            type = UnitEventType.Death
        });
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
        EventBus.Raise(new UnitEvent
        {
            source = this,
            type = UnitEventType.ItemUse
        });
    }

    public virtual void StartTurn()
    {
        currentAP = MaxAP;
        EventBus.Raise(new UnitEvent
        {
            source = this,
            type = UnitEventType.TurnStart
        });
        statusController?.OnTurnStart();
    }

    public virtual void ContinueTurn()
    {
        TurnEvent.OnUnitContinueTurn?.Invoke(this);
    }

    public virtual void EndTurn()
    {
        statusController?.OnTurnEnd();
        EventBus.Raise(new UnitEvent
        {
            source = this,
            type = UnitEventType.TurnEnd
        });
    }

    public virtual void ActionResolved()
    {
        TurnEvent.OnUnitActionResolved?.Invoke(this);
        EventBus.Raise(new UnitEvent
        {
            source = this,
            type = UnitEventType.ActionResolved
        });
    }

    public virtual bool SpendAP(int amount)
    {
        if (amount < 0) return false;

        if (currentAP >= amount)
        {
            currentAP -= amount;
            return true;
        }
        return false;
    }
}