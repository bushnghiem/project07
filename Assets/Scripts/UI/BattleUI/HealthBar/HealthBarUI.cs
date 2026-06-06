using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image healthFill;
    [SerializeField] private TMP_Text shieldText;

    [Header("Follow Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);
    [SerializeField] private float followSpeed = 10f;

    private HealthComponent health;
    private Transform target;
    private Camera cam;

    private System.Action<float, float> onHealthChanged;
    private System.Action<float, float> onMaxHealthChanged;
    private System.Action<int> onShieldChanged;
    private System.Action onDeath;

    public void Initialize(HealthComponent health, Transform target)
    {
        this.health = health;
        this.target = target;

        cam = Camera.main;

        // Cache delegates so unsubscribe works correctly
        onHealthChanged = (_, __) => Refresh();
        onMaxHealthChanged = (_, __) => Refresh();
        onShieldChanged = _ => Refresh();

        health.OnHealthChanged += onHealthChanged;
        health.OnMaxHealthChanged += onMaxHealthChanged;
        health.OnShieldChanged += onShieldChanged;
        onDeath = HandleDeath;
        health.OnDeath += onDeath;

        Refresh();
    }

    private void OnDestroy()
    {
        if (health == null) return;

        health.OnHealthChanged -= onHealthChanged;
        health.OnMaxHealthChanged -= onMaxHealthChanged;
        health.OnShieldChanged -= onShieldChanged;
        health.OnDeath -= onDeath;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            Time.deltaTime * followSpeed
        );

        if (cam == null)
            cam = Camera.main;

        if (cam != null)
        {
            transform.forward = cam.transform.forward;
        }
    }

    private void Refresh()
    {
        if (health == null) return;

        float hpPercent =
            health.GetCurrentHealth() /
            Mathf.Max(1f, health.GetMaxHealth());

        healthFill.fillAmount = hpPercent;

        int shield = health.GetShield();

        shieldText.text =
            shield > 0 ? $"🛡 {shield}" : "";
    }

    private void HandleDeath()
    {
        Destroy(gameObject);
    }
}