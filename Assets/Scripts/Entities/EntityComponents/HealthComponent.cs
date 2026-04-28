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

        Debug.Log($"[SET HP] {oldHealth} → {newCurrentHealth} frame={Time.frameCount}");

        currentHealth = Mathf.Clamp(newCurrentHealth, 0f, maxHealth);

        Debug.Log($"[CLAMPED HP] now={currentHealth}");

        OnHealthChanged?.Invoke(oldHealth, currentHealth);

        if (currentHealth <= 0f && !isDead)
        {
            isDead = true;
            Debug.Log($"[DEATH TRIGGERED]");
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
        if (isDead)
        {
            Debug.Log($"[HURT IGNORED - DEAD] dmg={damage} frame={Time.frameCount}");
            return;
        }

        Debug.Log($"[HURT ENTER] dmg={damage} hpBefore={currentHealth} shield={shield} frame={Time.frameCount}");

        if (shield > 0)
        {
            shield--;
            Debug.Log($"[SHIELD BLOCK] remainingShield={shield} frame={Time.frameCount}");
        }
        else
        {
            SetCurrentHealth(currentHealth - damage);
            OnDamaged?.Invoke(damage);

            Debug.Log($"[HURT APPLIED] dmg={damage} hpAfter={currentHealth} frame={Time.frameCount}");
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
