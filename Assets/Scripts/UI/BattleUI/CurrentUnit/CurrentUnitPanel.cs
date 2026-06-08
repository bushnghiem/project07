using TMPro;
using UnityEngine;

public class CurrentUnitPanel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text turnText;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text shieldText;
    [SerializeField] private TMP_Text apText;
    [SerializeField] private TMP_Text activeItemText;
    [SerializeField] private TMP_Text cooldownText;

    [Header("Status UI")]
    [SerializeField] private Transform statusContainer;
    [SerializeField] private StatusEffectRowUI statusRowPrefab;

    private UnitBase currentUnit;
    private HealthComponent currentHealth;
    private StatusEffectController currentStatus;

    private void OnEnable()
    {
        TurnEvent.OnUnitTurnStart += HandleTurnStart;
        TurnEvent.OnUnitActionResolved += HandleActionResolved;
    }

    private void OnDisable()
    {
        TurnEvent.OnUnitTurnStart -= HandleTurnStart;
        TurnEvent.OnUnitActionResolved -= HandleActionResolved;

        UnhookUnit();
    }

    // ----------------------------
    // UNIT SWITCH
    // ----------------------------
    private void HandleTurnStart(Unit unit)
    {
        if (unit is not UnitBase ub) return;
        SetUnit(ub);
    }

    private void SetUnit(UnitBase unit)
    {
        UnhookUnit();

        currentUnit = unit;
        currentHealth = unit.GetComponent<HealthComponent>();
        currentStatus = unit.GetComponent<StatusEffectController>();

        if (currentHealth != null)
        {
            currentHealth.OnHealthChanged += OnHealthChanged;
            currentHealth.OnShieldChanged += OnShieldChanged;
        }

        if (currentStatus != null)
        {
            currentStatus.OnEffectsChanged += RefreshStatus;
        }

        RefreshAll();
    }

    private void UnhookUnit()
    {
        if (currentHealth != null)
        {
            currentHealth.OnHealthChanged -= OnHealthChanged;
            currentHealth.OnShieldChanged -= OnShieldChanged;
        }

        if (currentStatus != null)
        {
            currentStatus.OnEffectsChanged -= RefreshStatus;
        }
    }

    // ----------------------------
    // ACTION UPDATES
    // ----------------------------
    private void HandleActionResolved(Unit unit)
    {
        if (unit != currentUnit) return;
        RefreshAll();
    }

    // ----------------------------
    // HEALTH EVENTS
    // ----------------------------
    private void OnHealthChanged(float oldHp, float newHp) => RefreshBasic();
    private void OnShieldChanged(int shield) => RefreshBasic();

    // ----------------------------
    // REFRESH FUNCTIONS
    // ----------------------------
    private void RefreshAll()
    {
        RefreshBasic();
        RefreshStatus();
    }

    private void RefreshBasic()
    {
        if (currentUnit == null) return;

        nameText.text = currentUnit.RunData.uniqueID;
        turnText.text = currentUnit.IsPlayerControllable ? "PLAYER TURN" : "ENEMY TURN";
        hpText.text = $"HP {currentUnit.CurrentHealth:0}/{currentUnit.MaxHealth:0}";
        shieldText.text = $"Shield {currentUnit.CurrentShield}";
        apText.text = $"AP {currentUnit.CurrentAP}/{(int)currentUnit.GetStat(ShipStatType.ActionPoints)}";

        if (currentUnit.ActiveItem != null)
        {
            activeItemText.text = currentUnit.ActiveItem.itemData.itemName;

            int cd = currentUnit.ActiveItem.GetRemainingCooldown();

            cooldownText.text =
                cd <= 0
                ? "Ready"
                : $"Cooldown: {cd}";
        }
        else
        {
            activeItemText.text = "No Active Item";
            cooldownText.text = "-";
        }
    }

    private void RefreshStatus()
    {
        if (currentStatus == null) return;

        foreach (Transform child in statusContainer)
            Destroy(child.gameObject);

        foreach (var effect in currentStatus.ActiveEffects)
        {
            var row = Instantiate(statusRowPrefab, statusContainer);
            row.Setup(effect);
        }
    }
}