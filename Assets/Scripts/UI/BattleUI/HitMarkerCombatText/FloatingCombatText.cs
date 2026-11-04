
using UnityEngine;
using TMPro;

public class FloatingCombatText : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshPro text;

    [Header("Size")]
    [SerializeField] private float minSize = 1f;
    [SerializeField] private float maxSize = 2.5f;
    [SerializeField] private float magnitudeForMaxSize = 100f;

    [Header("Lifetime")]
    [SerializeField] private float minLifetime = 0.8f;
    [SerializeField] private float maxLifetime = 2f;
    [SerializeField] private float magnitudeForMaxLifetime = 100f;

    [Header("Movement")]
    [SerializeField] private float minMovementSpeed = 0.8f;
    [SerializeField] private float maxMovementSpeed = 2f;

    [Header("Movement Size Scaling")]
    [SerializeField] private float minSizeSpeedMultiplier = 0.4f;
    [SerializeField] private float maxSizeSpeedMultiplier = 1.5f;

    [Header("Animation")]
    [SerializeField] private float scaleInDuration = 0.12f;
    [SerializeField] private float fadeStart = 0.5f;

    [Header("Status Effects")]
    [SerializeField] private float statusSizeMultiplier = 0.55f;
    [SerializeField] private float minimumStatusLifetime = 1.2f;

    private float elapsed;
    private float lifetime;

    private float targetSize;
    private Vector3 startScale;

    private Vector3 velocity;

    private Color originalColor;

    public void Initialize(
        string message,
        Color color,
        float magnitude,
        bool isStatus = false)
    {
        if (text == null)
            text = GetComponentInChildren<TextMeshPro>();

        if (text == null)
        {
            Debug.LogError(
                "FloatingCombatText has no TextMeshPro!"
            );

            Destroy(gameObject);
            return;
        }

        elapsed = 0f;

        text.text = message;
        text.color = color;

        originalColor = color;

        // Calculate text size.
        float sizeRatio = Mathf.Clamp01(
            magnitude / magnitudeForMaxSize
        );

        targetSize = Mathf.Lerp(
            minSize,
            maxSize,
            sizeRatio
        );

        // Calculate lifetime.
        lifetime = Mathf.Lerp(
            minLifetime,
            maxLifetime,
            Mathf.Clamp01(
                magnitude / magnitudeForMaxLifetime
            )
        );

        // Status effects are smaller and remain visible longer.
        if (isStatus)
        {
            targetSize *= statusSizeMultiplier;

            lifetime = Mathf.Max(
                lifetime,
                minimumStatusLifetime
            );
        }

        // Initial scale for pop-in animation.
        startScale = Vector3.one * (targetSize * 0.3f);

        transform.localScale = startScale;

        // Random movement direction on X/Z plane.
        Vector2 randomDirection =
            Random.insideUnitCircle.normalized;

        if (randomDirection.sqrMagnitude < 0.01f)
            randomDirection = Vector2.up;

        // Random movement speed.
        float randomSpeed = Random.Range(
            minMovementSpeed,
            maxMovementSpeed
        );

        // Larger text moves more slowly.
        float sizeMultiplier = Mathf.Clamp(
            1f / targetSize,
            minSizeSpeedMultiplier,
            maxSizeSpeedMultiplier
        );

        randomSpeed *= sizeMultiplier;

        // Final movement velocity.
        velocity = new Vector3(
            randomDirection.x,
            0f,
            randomDirection.y
        ) * randomSpeed;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        float normalizedTime = Mathf.Clamp01(
            elapsed / lifetime
        );

        // Gradually slow down movement.
        float movementMultiplier =
            1f - normalizedTime * 0.5f;

        transform.position +=
            velocity *
            movementMultiplier *
            Time.deltaTime;

        // Scale animation.
        float scaleProgress = Mathf.Clamp01(
            elapsed / scaleInDuration
        );

        scaleProgress = Mathf.SmoothStep(
            0f,
            1f,
            scaleProgress
        );

        transform.localScale = Vector3.Lerp(
            startScale,
            Vector3.one * targetSize,
            scaleProgress
        );

        // Fade out.
        float alpha = 1f;

        if (normalizedTime > fadeStart)
        {
            alpha = Mathf.Lerp(
                1f,
                0f,
                (normalizedTime - fadeStart) /
                (1f - fadeStart)
            );
        }

        Color color = originalColor;
        color.a = alpha;

        text.color = color;

        // Keep the fixed top-down text orientation.
        transform.rotation = Quaternion.Euler(
            90f,
            0f,
            0f
        );

        // Destroy after lifetime.
        if (elapsed >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
