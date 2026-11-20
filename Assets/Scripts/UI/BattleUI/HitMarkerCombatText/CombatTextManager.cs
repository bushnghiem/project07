
using UnityEngine;

public class CombatTextManager : MonoBehaviour
{
    public static CombatTextManager Instance { get; private set; }

    [Header("Prefab")]
    [SerializeField] private FloatingCombatText textPrefab;

    [Header("Spawn Position")]
    [SerializeField]
    private Vector3 spawnOffset =
        new Vector3(0f, 1.5f, 0f);

    [Header("Random Spawn Offset")]
    [SerializeField] private float randomSpawnRadius = 0.6f;

    [Header("Colors")]
    [SerializeField]
    private Color damageColor =
        new Color(1f, 0.25f, 0.2f);

    [SerializeField]
    private Color criticalColor =
        new Color(1f, 0.8f, 0.2f);

    [SerializeField]
    private Color healingColor =
        new Color(0.25f, 1f, 0.4f);

    [SerializeField]
    private Color shieldGainColor =
        new Color(0.2f, 0.85f, 1f);

    [SerializeField]
    private Color shieldLossColor =
        new Color(0.3f, 0.55f, 1f);

    [SerializeField]
    private Color buffColor =
        new Color(0.3f, 0.65f, 1f);

    [SerializeField]
    private Color debuffColor =
        new Color(1f, 0.5f, 0.15f);

    [SerializeField]
    private Color statusRemovedColor =
        new Color(0.65f, 0.65f, 0.65f);

    [SerializeField]
    private Color actionFailedColor =
        new Color(1f, 0.3f, 0.3f);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void ShowDamage(Vector3 position, float amount)
    {
        if (amount <= 0f) return;

        Spawn(
            position,
            $"-{Mathf.RoundToInt(amount)}",
            damageColor,
            amount
        );
    }

    public void ShowCriticalDamage(Vector3 position, float amount)
    {
        if (amount <= 0f) return;

        Spawn(
            position,
            $"-{Mathf.RoundToInt(amount)}!",
            criticalColor,
            amount * 1.25f
        );
    }

    public void ShowHealing(Vector3 position, float amount)
    {
        if (amount <= 0f) return;

        Spawn(
            position,
            $"+{Mathf.RoundToInt(amount)}",
            healingColor,
            amount
        );
    }

    public void ShowShieldGain(Vector3 position, int amount)
    {
        if (amount <= 0) return;

        Spawn(
            position,
            $"+{amount} SHIELD",
            shieldGainColor,
            amount * 10f
        );
    }

    public void ShowShieldLoss(Vector3 position, int amount)
    {
        if (amount <= 0) return;

        Spawn(
            position,
            $"-{amount} SHIELD",
            shieldLossColor,
            amount * 10f
        );
    }

    public void ShowStatus(
        Vector3 position,
        StatusEffectInstance effect)
    {
        if (effect == null || effect.data == null)
            return;

        string message = effect.data.displayName;

        if (effect.Stacks > 1)
            message += $" x{effect.Stacks}";

        message += $" ({effect.RemainingDuration} turns)";

        Color color = effect.data.polarity switch
        {
            StatusEffectPolarity.Buff => buffColor,
            StatusEffectPolarity.Debuff => debuffColor,
            _ => Color.white
        };

        Spawn(
            position,
            message,
            color,
            15f,
            true
        );
    }

    public void ShowStatusRemoved(
        Vector3 position,
        string effectName)
    {
        Spawn(
            position,
            effectName + " REMOVED",
            statusRemovedColor,
            15f,
            true
        );
    }

    public void ShowActionFailed(Vector3 position, string message)
    {
        Spawn(
            position,
            message,
            actionFailedColor,
            15f,
            true
        );
    }

    public void ShowNotEnoughCharges(Vector3 position)
    {
        ShowActionFailed(position, "NOT ENOUGH CHARGES");
    }

    public void ShowNoActiveItem(Vector3 position)
    {
        ShowActionFailed(position, "NO ACTIVE ITEM");
    }

    public void ShowItemOnCooldown(Vector3 position)
    {
        ShowActionFailed(position, "ITEM ON COOLDOWN");
    }

    private void Spawn(
        Vector3 position,
        string message,
        Color color,
        float magnitude,
        bool isStatus = false)
    {
        if (textPrefab == null)
            return;

        // Random spawn position on X/Z plane.
        Vector2 randomOffset =
            Random.insideUnitCircle * randomSpawnRadius;

        Vector3 spawnPosition = position + spawnOffset;

        spawnPosition += new Vector3(
            randomOffset.x,
            0f,
            randomOffset.y
        );

        FloatingCombatText instance = Instantiate(
            textPrefab,
            spawnPosition,
            Quaternion.identity
        );

        instance.Initialize(
            message,
            color,
            magnitude,
            isStatus
        );
    }
}
