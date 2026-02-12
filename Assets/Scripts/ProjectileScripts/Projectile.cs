using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rb;
    public float linearDamping = 2.0f;
    public float angularDamping = 2.0f;
    public float lifeTime = 5.0f;

    public HealthComponent healthComp;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthComp = GetComponent<HealthComponent>();
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
        DeathEvent.OnEntityDeath?.Invoke(transform.position, gameObject);
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
