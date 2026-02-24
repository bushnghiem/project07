using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private int shield;
    public bool isDead = false;

    public event Action<float> OnDamaged;
    public event Action<float> OnHealed;
    public event Action OnDeath;
    public event Action OnFullHealth;
    public event Action<float, float> OnHealthChanged;
    public event Action<float, float> OnMaxHealthChanged;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrentHealth(float newCurrentHealth)
    {
        float oldHealth = currentHealth;
        currentHealth = newCurrentHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        OnHealthChanged?.Invoke(oldHealth, currentHealth);

        if (currentHealth <= 0f && !isDead)
        {
            isDead = true;
            OnDeath?.Invoke();
        }
        if (currentHealth == maxHealth)
        {
            OnFullHealth?.Invoke();
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        float oldMaxhealth = maxHealth;
        maxHealth = newMaxHealth;
        OnMaxHealthChanged?.Invoke(oldMaxhealth, maxHealth);
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetShield(int newShield)
    {
        shield = newShield;
    }

    public int GetShield()
    {
        return shield;
    }

    public void Hurt(float damage)
    {
        if (isDead) return;

        if (shield > 0)
        {
            shield--;
            Debug.Log("shield point lost");
        }
        else
        {
            SetCurrentHealth(currentHealth - damage);
            OnDamaged?.Invoke(damage);
        }
    }

    public void Heal(float gain)
    {
        if (isDead) return;

        SetCurrentHealth(currentHealth + gain);
        OnHealed?.Invoke(gain);
    }

    public void addShield(int shieldGain)
    {
        if (isDead) return;

        SetShield(shield + shieldGain);
    }
}
