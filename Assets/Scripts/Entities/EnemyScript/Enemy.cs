using UnityEngine;

public class Enemy : MonoBehaviour, Entity
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
        Debug.Log($"Enemy took {damage} damage");
    }

    private void HandleHealed(float amount)
    {
        Debug.Log($"Enemy healed {amount}");
    }

    private void HandleDeath()
    {
        DeathEvent.OnEntityDeath?.Invoke(this);
    }

    public void Attack(Player target)
    {
        Debug.Log("Enemy attacks!");
        TurnEvent.OnEnemyTurnEnd?.Invoke(this);
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
