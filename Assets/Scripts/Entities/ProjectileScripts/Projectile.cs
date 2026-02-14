using UnityEngine;

public class Projectile : MonoBehaviour, Entity
{
    Rigidbody rb;
    public float linearDamping = 2.0f;
    public float angularDamping = 2.0f;
    public float lifeTime = 5.0f;

    public HealthComponent healthComp;

    public Vector3 Position => transform.position;
    public bool isDead => healthComp.isDead;

    private Collider[] colliders;
    private Renderer[] renderers;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthComp = GetComponent<HealthComponent>();
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
        Debug.Log($"Projectile took {damage} damage");
    }

    private void HandleHealed(float amount)
    {
        Debug.Log($"Projectile healed {amount}");
    }

    private void HandleDeath()
    {
        Kill();
        DeathEvent.OnEntityDeath?.Invoke(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearDamping = linearDamping;
        rb.angularDamping = angularDamping;
        Invoke("HandleDeath", lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Fling(Vector3 direction, float forceStrength)
    {
        rb.AddForce(direction * forceStrength, ForceMode.Impulse);
    }
}
