using System;
using UnityEngine;

public class ChargeComponent : MonoBehaviour
{
    [Header("Charges")]
    [SerializeField] private int currentCharges;
    [SerializeField] private int maxCharges;

    public event Action<int, int> OnChargeChanged;
    public event Action<int, int> OnMaxChargeChanged;
    public event Action<int> OnChargesSpent;
    public event Action<int> OnChargesGained;
    public event Action OnOutOfCharges;
    public event Action OnFullCharges;

    public void SetCurrentCharges(int newCharges)
    {
        int old = currentCharges;

        currentCharges = Mathf.Clamp(newCharges, 0, maxCharges);

        OnChargeChanged?.Invoke(old, currentCharges);

        if (currentCharges == 0)
            OnOutOfCharges?.Invoke();

        if (currentCharges == maxCharges)
            OnFullCharges?.Invoke();
    }

    public int GetCurrentCharges()
    {
        return currentCharges;
    }

    public void SetMaxCharges(int newMax)
    {
        int old = maxCharges;

        maxCharges = Mathf.Max(0, newMax);

        if (currentCharges > maxCharges)
            currentCharges = maxCharges;

        OnMaxChargeChanged?.Invoke(old, maxCharges);
    }

    public int GetMaxCharges()
    {
        return maxCharges;
    }

    public bool CanSpend(int amount)
    {
        if (amount <= 0)
            return true;

        return currentCharges >= amount;
    }

    public bool Spend(int amount)
    {
        if (!CanSpend(amount))
            return false;

        SetCurrentCharges(currentCharges - amount);

        OnChargesSpent?.Invoke(amount);

        return true;
    }

    public void Gain(int amount)
    {
        if (amount <= 0)
            return;

        SetCurrentCharges(currentCharges + amount);

        OnChargesGained?.Invoke(amount);
    }

    public void Refill()
    {
        SetCurrentCharges(maxCharges);
    }
}