using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    [Header("Shield")]
    [SerializeField] private int shield;

    public bool isDead = false;

    public event Action<float> OnDamaged;
    public event Action<float> OnHealed;
    public event Action OnDeath;
    public event Action OnFullHealth;

    public event Action<float, float> OnHealthChanged;
    public event Action<float, float> OnMaxHealthChanged;
    public event Action<int> OnShieldChanged;

    public void SetCurrentHealth(
        float newCurrentHealth)
    {
        float oldHealth = currentHealth;

        currentHealth = Mathf.Clamp(
            newCurrentHealth,
            0f,
            maxHealth
        );

        if (!Mathf.Approximately(
            oldHealth,
            currentHealth))
        {
            OnHealthChanged?.Invoke(
                oldHealth,
                currentHealth
            );
        }

        if (currentHealth <= 0f && !isDead)
        {
            isDead = true;
            OnDeath?.Invoke();
        }

        if (Mathf.Approximately(
            currentHealth,
            maxHealth))
        {
            OnFullHealth?.Invoke();
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetMaxHealth(
        float newMaxHealth)
    {
        newMaxHealth =
            Mathf.Max(0f, newMaxHealth);

        float oldMaxHealth = maxHealth;

        maxHealth = newMaxHealth;

        // Max HP went down.
        // Current HP cannot remain above it.
        if (currentHealth > maxHealth)
        {
            SetCurrentHealth(maxHealth);
        }

        if (!Mathf.Approximately(
            oldMaxHealth,
            maxHealth))
        {
            OnMaxHealthChanged?.Invoke(
                oldMaxHealth,
                maxHealth
            );
        }
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetShield(int newShield)
    {
        shield = Mathf.Max(0, newShield);

        Debug.Log("set shield to " + shield);

        OnShieldChanged?.Invoke(shield);
    }

    public int GetShield()
    {
        return shield;
    }

    public void Hurt(DamageInfo damageInfo)
    {
        if (isDead)
            return;

        float damage = damageInfo.Amount;

        bool blockedByShield =
            shield > 0 &&
            !damageInfo.BypassShields;

        if (blockedByShield)
        {
            shield--;

            OnShieldChanged?.Invoke(
                shield
            );

            return;
        }

        SetCurrentHealth(
            currentHealth - damage
        );

        OnDamaged?.Invoke(damage);
    }

    public void Heal(float gain)
    {
        if (isDead)
            return;

        SetCurrentHealth(
            currentHealth + gain
        );

        OnHealed?.Invoke(gain);
    }

    public void addShield(int shieldGain)
    {
        if (isDead)
            return;

        SetShield(
            shield + shieldGain
        );
    }
}
