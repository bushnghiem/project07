using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Intensity")]
    [SerializeField] private float maxIntensity = 1.5f;

    [Header("Shake Duration")]
    [SerializeField] private float shakeDuration = 0.4f;

    [Header("Shake Frequency")]
    [SerializeField] private float frequency = 25f;

    [Header("Position")]
    [SerializeField] private float positionMultiplier = 0.15f;

    [Header("Decay")]
    [SerializeField]
    private AnimationCurve shakeCurve =
        new AnimationCurve(
            new Keyframe(0f, 1f),
            new Keyframe(0.15f, 0.65f),
            new Keyframe(0.5f, 0.2f),
            new Keyframe(1f, 0f)
        );

    private float currentIntensity;
    private float shakeTimer;

    private Vector3 initialPosition;

    private float seedX;
    private float seedZ;

    private void Awake()
    {
        initialPosition = transform.localPosition;

        seedX = Random.Range(0f, 1000f);
        seedZ = Random.Range(0f, 1000f);
    }

    private void OnEnable()
    {
        CameraEvent.ShakeCamera += AddShake;
    }

    private void OnDisable()
    {
        CameraEvent.ShakeCamera -= AddShake;

        currentIntensity = 0f;
        shakeTimer = 0f;

        ResetTransform();
    }

    public void AddShake(float intensity)
    {
        currentIntensity = Mathf.Clamp(
            currentIntensity + intensity,
            0f,
            maxIntensity
        );

        // Refresh shake duration
        shakeTimer = shakeDuration;
    }

    private void LateUpdate()
    {
        if (shakeTimer <= 0f || currentIntensity <= 0.001f)
        {
            currentIntensity = 0f;
            ResetTransform();
            return;
        }

        shakeTimer -= Time.deltaTime;

        // Calculate shake progress
        float progress =
            1f - Mathf.Clamp01(shakeTimer / shakeDuration);

        // Evaluate intensity over time
        float strength =
            shakeCurve.Evaluate(progress) * currentIntensity;

        float time = Time.time * frequency;

        // Generate smooth noise
        float x = Mathf.PerlinNoise(seedX, time) * 2f - 1f;
        float z = Mathf.PerlinNoise(seedZ, time) * 2f - 1f;

        // Only shake on X and Z
        Vector3 positionOffset = new Vector3(
            x,
            0f,
            z
        ) * strength * positionMultiplier;

        // Apply shake relative to original position
        transform.localPosition =
            initialPosition + positionOffset;
    }

    private void ResetTransform()
    {
        transform.localPosition = initialPosition;
    }
}
