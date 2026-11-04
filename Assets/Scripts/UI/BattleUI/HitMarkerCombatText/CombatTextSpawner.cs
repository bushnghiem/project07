
using UnityEngine;

public class CombatTextSpawner : MonoBehaviour
{
    private HealthComponent health;
    private StatusEffectController statusController;

    private void Awake()
    {
        health = GetComponent<HealthComponent>();
        statusController = GetComponent<StatusEffectController>();
    }

    private void OnEnable()
    {
        if (health != null)
        {
            health.OnDamaged += HandleDamage;
            health.OnHealed += HandleHealing;
            health.OnShieldGained += HandleShieldGained;
            health.OnShieldLost += HandleShieldLost;
        }

        if (statusController != null)
        {
            statusController.OnEffectApplied += HandleStatusApplied;
            statusController.OnEffectRemoved += HandleStatusRemoved;
        }
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.OnDamaged -= HandleDamage;
            health.OnHealed -= HandleHealing;
            health.OnShieldGained -= HandleShieldGained;
            health.OnShieldLost -= HandleShieldLost;
        }

        if (statusController != null)
        {
            statusController.OnEffectApplied -= HandleStatusApplied;
            statusController.OnEffectRemoved -= HandleStatusRemoved;
        }
    }

    private void HandleDamage(float amount)
    {
        CombatTextManager.Instance?.ShowDamage(
            transform.position,
            amount
        );
    }

    private void HandleHealing(float amount)
    {
        CombatTextManager.Instance?.ShowHealing(
            transform.position,
            amount
        );
    }

    private void HandleShieldGained(int amount)
    {
        CombatTextManager.Instance?.ShowShieldGain(
            transform.position,
            amount
        );
    }

    private void HandleShieldLost(int amount)
    {
        CombatTextManager.Instance?.ShowShieldLoss(
            transform.position,
            amount
        );
    }

    private void HandleStatusApplied(StatusEffectInstance effect)
    {
        CombatTextManager.Instance?.ShowStatus(
            transform.position,
            effect
        );
    }

    private void HandleStatusRemoved(StatusEffectInstance effect)
    {
        if (effect == null || effect.data == null)
            return;

        CombatTextManager.Instance?.ShowStatusRemoved(
            transform.position,
            effect.data.displayName
        );
    }
}
