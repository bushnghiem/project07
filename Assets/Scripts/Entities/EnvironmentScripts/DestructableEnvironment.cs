using UnityEngine;

public class DestructableEnvironment : MonoBehaviour, Entity, IInspectable
{
    public HealthComponent healthComp;
    public DamageOnCollision collisionDamageComp;

    public string displayName;

    public Vector3 Position => transform.position;
    public bool isDead => healthComp.isDead;

    private Collider[] colliders;
    private Renderer[] renderers;

    private ActionContext actionContext;

    public ActionContext ActionContext => actionContext;

    public void SetActionContext(ActionContext context)
    {
        actionContext = context;
    }

    public UnitBase Instigator => null;

    public string DisplayName => displayName;

    private void Awake()
    {
        healthComp = GetComponent<HealthComponent>();
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

    public void Hurt(DamageInfo damageInfo)
    {
        healthComp.Hurt(damageInfo);
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
        Debug.Log($"Wall took {damage} damage");
    }

    private void HandleHealed(float amount)
    {
        Debug.Log($"Wall healed {amount}");
    }

    private void HandleDeath()
    {
        Kill();
        DeathEvent.OnEntityDeath?.Invoke(this);
    }

    public InspectionData GetInspectionData()
    {
        return new InspectionData
        {
            Name = gameObject.name,

            CurrentHP = healthComp.GetCurrentHealth(),
            MaxHP = healthComp.GetMaxHealth(),

            Shield = healthComp.GetShield(),

            CollisionDamage = collisionDamageComp.ContactDamage,

            ExtraInfo = "Environment"
        };
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
