using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour, Unit, IInspectable
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
    protected ChargeComponent chargeComp;
    protected DamageOnCollision collisionDamageComp;
    protected EffectController effectController;
    protected StatusEffectController statusController;

    protected ActiveItemInstance activeItem;
    protected Projectile projectile;
    protected ProjectileItem projectileItem;
    protected List<PassiveItemInstance> passiveItems = new List<PassiveItemInstance>();

    protected List<Effect> onShootEffectsFromProjectile = new();
    protected List<Effect> projectileEffectModifiers = new();
    public IReadOnlyList<Effect> ProjectileEffectModifiers => projectileEffectModifiers;

    protected List<AppliedStatusEffect> projectileCollisionStatusModifiers = new();
    public IReadOnlyList<AppliedStatusEffect> ProjectileCollisionStatusModifiers
        => projectileCollisionStatusModifiers;

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

    public ActiveItemInstance ActiveItem => activeItem;

    public Projectile Projectile => projectile;

    public ProjectileItem ProjectileItem => projectileItem;

    public IReadOnlyList<PassiveItemInstance> PassiveItems => passiveItems;

    public ShipRunData RunData => runData;

    public float CurrentHealth => healthComp.GetCurrentHealth();
    public float MaxHealth => healthComp.GetMaxHealth();

    public int CurrentCharges => chargeComp.GetCurrentCharges();
    public int MaxCharges => chargeComp.GetMaxCharges();

    public int CurrentShield => healthComp.GetShield();

    public ShipTemplate Template => template;

    public string DisplayName => RunData.uniqueID;

    protected SphereCollider sphereCollider;
    protected ShipAudioComponent audioComp;

    protected List<ShotModifier> shotModifiers = new();

    public IReadOnlyList<ShotModifier> ShotModifiers => shotModifiers;

    private ActionContext actionContext;

    public ActionContext ActionContext => actionContext;

    public void AssignActionContext(ActionContext context)
    {
        actionContext = context;

        if (context != null)
        {
            context.OnContextFinished += ClearActionContext;
        }
    }

    private void ClearActionContext()
    {
        actionContext = null;
    }

    public UnitBase Instigator => this;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthComp = GetComponent<HealthComponent>();
        chargeComp = GetComponent<ChargeComponent>();
        collisionDamageComp = GetComponent<DamageOnCollision>();
        effectController = GetComponent<EffectController>();
        statusController = GetComponent<StatusEffectController>();
        sphereCollider = GetComponent<SphereCollider>();
        audioComp = GetComponent<ShipAudioComponent>();
    }

    public virtual void Initialize(ShipRunData data)
    {
        runData = data;
        template = shipDatabase.GetTemplate(runData.templateID);
        audioComp.SetUp();
        ApplyStats();
        //collisionDamageComp.SetCollisionSounds(template.AudioData.Collision);
    }

    protected virtual void ApplyStats()
    {
        float maxHealth = GetStat(ShipStatType.MaxHealth);
        int maxCharges = Mathf.RoundToInt(GetStat(ShipStatType.MaxCharges));
        int shield = Mathf.RoundToInt(GetStat(ShipStatType.StartingShield));
        float mass = GetStat(ShipStatType.Mass);
        float collisionDamage = GetStat(ShipStatType.CollisionDamage);
        float collisionKnockback = GetStat(ShipStatType.CollisionKnockback);
        MaxAP = (int)GetStat(ShipStatType.ActionPoints);

        healthComp.SetMaxHealth(maxHealth);
        healthComp.SetShield(shield);
        chargeComp.SetMaxCharges(maxCharges);

        healthComp.SetCurrentHealth(
            runData.currentHealth > 0 ? runData.currentHealth : maxHealth
            );

        chargeComp.SetCurrentCharges(runData.currentCharges);

        rb.mass = mass;

        if (collisionDamageComp != null)
            collisionDamageComp.SetCollisionStats(collisionDamage, collisionKnockback);

        if (sphereCollider != null)
        {
            sphereCollider.radius = template.CollisionRadius;
        }
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

    public void SetCollisionStatusEffects(List<AppliedStatusEffect> newStatusEffects)
    {
        collisionDamageComp.statusEffects = newStatusEffects;
    }

    public void AddCollisionStatusEffects(List<AppliedStatusEffect> newStatusEffects)
    {
        foreach (var status in newStatusEffects)
        {
            collisionDamageComp.statusEffects.Add(status);
        }
    }

    public void RemoveCollisionStatusEffects(List<AppliedStatusEffect> toRemove)
    {
        foreach (var remove in toRemove)
        {
            collisionDamageComp.statusEffects.RemoveAll(s =>
                s.sourceID == remove.sourceID
            );
        }
    }

    public bool RemoveItemFromRunData(Item item)
    {
        if (runData.items == null || item == null)
            return false;

        int index = runData.items.FindIndex(i => i.itemID == item.itemID);

        if (index < 0)
            return false;

        runData.items.RemoveAt(index);
        return true;
    }

    public virtual void EquipPassive(PassiveItem passive)
    {
        var instance = new PassiveItemInstance(passive);
        instance.Apply(this);
        passiveItems.Add(instance);
    }

    public virtual bool RemovePassive(PassiveItem passive)
    {
        var instance = passiveItems.Find(p => p.itemData == passive);

        if (instance == null)
            return false;

        instance.Remove(this);
        passiveItems.Remove(instance);

        RemoveItemFromRunData(passive);

        return true;
    }

    public virtual void EquipActive(ActiveItem item)
    {
        activeItem = new ActiveItemInstance(item);
    }

    public virtual bool RemoveActive(ActiveItem item)
    {
        if (activeItem == null)
            return false;

        if (activeItem.itemData != item)
            return false;

        activeItem = null;

        RemoveItemFromRunData(item);

        return true;
    }

    public virtual void EquipProjectile(ProjectileItem projectileItem)
    {
        RemoveOnShootProjectileInjectedEffects(effectController);

        this.projectile = projectileItem.projectile;
        this.projectileItem = projectileItem;

        if (effectController == null)
            return;

        if (this.projectile.effects != null)
        {
            foreach (var effect in this.projectile.effects)
            {
                if (effect.trigger == EffectTrigger.OnShoot)
                {
                    Effect runtimeEffect = Instantiate(effect);
                    effectController.effects.Add(runtimeEffect);
                    onShootEffectsFromProjectile.Add(runtimeEffect);
                }
            }
        }
    }


    public virtual bool RemoveProjectile(ProjectileItem item)
    {
        if (projectileItem == null)
            return false;

        if (projectileItem != item)
            return false;

        RemoveOnShootProjectileInjectedEffects(effectController);

        projectile = null;
        projectileItem = null;

        RemoveItemFromRunData(item);

        return true;
    }

    protected void RemoveOnShootProjectileInjectedEffects(
    EffectController effectController)
    {
        foreach (var effect in onShootEffectsFromProjectile)
        {
            if (effectController != null)
                effectController.effects.Remove(effect);

            Destroy(effect);
        }

        onShootEffectsFromProjectile.Clear();
    }



    public void AddProjectileRuntimeEffect(Effect effect)
    {
        projectileEffectModifiers.Add(effect);
    }

    public void RemoveProjectileRuntimeEffect(Effect effect)
    {
        projectileEffectModifiers.Remove(effect);
    }

    public void AddProjectileCollisionStatus(AppliedStatusEffect effect)
    {
        projectileCollisionStatusModifiers.Add(effect);
    }

    public void RemoveProjectileCollisionStatus(AppliedStatusEffect effect)
    {
        projectileCollisionStatusModifiers.Remove(effect);
    }

    public void AddShotModifier(ShotModifier modifier)
    {
        shotModifiers.Add(modifier);
    }

    public void RemoveShotModifier(ShotModifier modifier)
    {
        shotModifiers.Remove(modifier);
    }

    public void AddItemToRunData(Item item)
    {
        if (item == null)
            return;

        if (runData.items == null)
            runData.items = new List<ItemSaveData>();

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

    public bool HasItem(Item item)
    {
        return GetItemCount(item) > 0;
    }

    public bool HasItem(string itemID)
    {
        if (string.IsNullOrEmpty(itemID))
            return false;

        Item item = GetItemFromID(itemID);

        return item != null && HasItem(item);
    }


    public int GetItemCount(Item item)
    {
        if (item == null)
            return 0;

        switch (item.slotType)
        {
            case ItemSlotType.Passive:
                int count = 0;

                foreach (var instance in passiveItems)
                {
                    if (instance.itemData == item)
                        count++;
                }

                return count;

            case ItemSlotType.Active:
                return activeItem != null &&
                       activeItem.itemData == item
                    ? 1
                    : 0;

            case ItemSlotType.Projectile:
                return projectileItem == item
                    ? 1
                    : 0;

            default:
                return 0;
        }
    }

    public int GetItemCount(string itemID)
    {
        if (string.IsNullOrEmpty(itemID))
            return 0;

        Item item = GetItemFromID(itemID);

        return item != null ? GetItemCount(item) : 0;
    }

    public virtual bool AcquireItem(Item item)
    {
        if (item == null)
            return false;

        if (item.slotType != ItemSlotType.Passive)
        {
            Item existingItem = GetItem(item.slotType);

            if (existingItem != null)
                Debug.Log("Not Passive Remove old Item");
                RemoveItem(existingItem);
        }

        item.OnAcquire(this);
        AddItemToRunData(item);

        return true;
    }

    public Item GetItem(ItemSlotType slotType)
    {
        switch (slotType)
        {
            case ItemSlotType.Active:
                return activeItem?.itemData;

            case ItemSlotType.Projectile:
                return projectileItem;

            default:
                return null;
        }
    }

    public virtual bool RemoveItem(Item item)
    {
        if (item == null)
            return false;

        switch (item)
        {
            case PassiveItem passive:
                return RemovePassive(passive);

            case ActiveItem active:
                return RemoveActive(active);

            case ProjectileItem projectile:
                return RemoveProjectile(projectile);

            default:
                return false;
        }
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

    public virtual void TriggerShootEffects(
    Vector3 direction,
    float force)
    {
        if (effectController == null)
            return;

        var context =
            new EffectContext(
                transform.position,
                gameObject,
                this,
                this
            );

        context.direction = direction;
        context.force = force;

        effectController.TriggerEffects(
            EffectTrigger.OnShoot,
            context
        );
    }

    public ActiveItemInstance GetActiveItem() => activeItem;
    public Projectile GetProjectile() => projectile;
    public float GetCurrentHealth() => healthComp.GetCurrentHealth();

    public virtual void Hurt(DamageInfo damageInfo)
    {
        float finalDamage = damageInfo.Amount;
        Debug.Log($"Initial Damage: {damageInfo.Amount}");

        // Category Res
        ShipStatType? categoryStat = DamageStatUtility.GetResistanceStat(damageInfo.Category);
        if (categoryStat.HasValue)
        {
            float resist = GetStat(categoryStat.Value);
            Debug.Log($"Category Resistance ({categoryStat.Value}): {resist}");

            finalDamage *= Mathf.Max(0f, 1f - resist);
            Debug.Log($"Damage after Category Resistance: {finalDamage}");
        }

        // Element Res
        ShipStatType? elementStat = DamageStatUtility.GetResistanceStat(damageInfo.Element);
        if (elementStat.HasValue)
        {
            float resist = GetStat(elementStat.Value);
            Debug.Log($"Element Resistance ({elementStat.Value}): {resist}");

            finalDamage *= Mathf.Max(0f, 1f - resist);
            Debug.Log($"Damage after Element Resistance: {finalDamage}");
        }

        DamageInfo resolvedDamage = damageInfo;
        resolvedDamage.Amount = finalDamage;

        Debug.Log($"Final Damage to Apply: {finalDamage}");
        healthComp.Hurt(resolvedDamage);

        EventBus.Raise(new UnitEvent
        {
            source = damageInfo.Instigator,
            damageSource = damageInfo.Source,
            target = this,
            type = UnitEventType.Hurt,
            value = finalDamage
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

    public virtual void Moved()
    {
        audioComp?.PlayMove();
        EventBus.Raise(new UnitEvent
        {
            source = this,
            type = UnitEventType.Move
        });
    }

    public void Shot()
    {
        audioComp?.PlayShoot();
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

    public bool SpendCharges(int amount)
    {
        return chargeComp.Spend(amount);
    }

    public void GainCharges(int amount)
    {
        chargeComp.Gain(amount);
    }

    public virtual InspectionData GetInspectionData()
    {
        return new InspectionData
        {
            Name = RunData.uniqueID,

            CurrentHP = CurrentHealth,
            MaxHP = MaxHealth,

            Shield = CurrentShield,

            CollisionDamage = GetStat(ShipStatType.CollisionDamage),

            ExtraInfo =
                IsPlayerControllable
                ? "Player Ship"
                : "Enemy Ship"
        };
    }
}