using UnityEngine;

public class Player : MonoBehaviour, Entity
{
    public ClickAndFling clickAndFlingComponent;
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
        Debug.Log($"Player took {damage} damage");
    }

    private void HandleHealed(float amount)
    {
        Debug.Log($"Player healed {amount}");
    }

    private void HandleDeath()
    {
        DeathEvent.OnEntityDeath?.Invoke(this);
    }

    public bool GetIsDead()
    {
        return healthComp.isDead;
    }

    public void EnablePlayer(bool value)
    {
        clickAndFlingComponent.SetFlingable(value);
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
