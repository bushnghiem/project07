using UnityEngine;

public class DestructableEnvironment : MonoBehaviour, Entity
{
    public HealthComponent healthComp;

    public Vector3 Position => transform.position;

    private void Awake()
    {
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
        Debug.Log($"Wall took {damage} damage");
    }

    private void HandleHealed(float amount)
    {
        Debug.Log($"Wall healed {amount}");
    }

    private void HandleDeath()
    {
        DeathEvent.OnEntityDeath?.Invoke(this);
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
