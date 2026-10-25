
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APIconDisplay : MonoBehaviour
{
    [Header("AP Icons")]
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Image iconPrefab;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color warningColor = Color.red;
    [SerializeField] private Color warningPreviewColor = Color.blue;

    [Header("Not Enough AP Animation")]
    [SerializeField] private float animationDuration = 0.4f;
    [SerializeField] private float shakeDistance = 8f;
    [SerializeField] private float shakeSpeed = 35f;

    private readonly List<Image> icons = new();

    private Coroutine warningCoroutine;

    private int previewAPCost;

    public void Refresh(int currentAP, int maxAP)
    {
        currentAP = Mathf.Clamp(currentAP, 0, maxAP);
        maxAP = Mathf.Max(0, maxAP);

        // Create additional icons if the unit needs them.
        while (icons.Count < maxAP)
        {
            Image icon = Instantiate(iconPrefab, iconContainer);
            icon.color = normalColor;
            icons.Add(icon);
        }

        // Reuse existing icons instead of destroying them.
        for (int i = 0; i < icons.Count; i++)
        {
            bool shouldShow = i < currentAP;

            icons[i].gameObject.SetActive(shouldShow);

            if (shouldShow)
                icons[i].color = normalColor;
        }

        // Stop animating unused icons when AP changes.
        if (warningCoroutine != null)
        {
            StopCoroutine(warningCoroutine);
            warningCoroutine = null;
        }

        ResetIconPositions();
        ApplyAPColors();
    }

    public void PlayNotEnoughAPAnimation(int cost)
    {
        if (warningCoroutine != null)
            StopCoroutine(warningCoroutine);

        warningCoroutine = StartCoroutine(
            NotEnoughAPAnimation(cost)
        );
    }

    private IEnumerator NotEnoughAPAnimation(int cost)
    {
        List<RectTransform> targets = new();
        List<Vector2> originalPositions = new();
        List<Image> targetImages = new();

        foreach (Image icon in icons)
        {
            if (!icon.gameObject.activeSelf)
                continue;

            RectTransform rect = icon.rectTransform;

            targets.Add(rect);
            originalPositions.Add(rect.anchoredPosition);
            targetImages.Add(icon);
        }

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(
                elapsed / animationDuration
            );

            // Strong shake at the start, settling at the end.
            float strength = 1f - progress;

            for (int i = 0; i < targets.Count; i++)
            {
                float offset = Mathf.Sin(
                    elapsed * shakeSpeed
                ) * shakeDistance * strength;

                targets[i].anchoredPosition =
                    originalPositions[i] +
                    new Vector2(offset, 0f);

                targetImages[i].color = Color.Lerp(
                    warningColor,
                    normalColor,
                    progress
                );
            }

            yield return null;
        }

        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].anchoredPosition = originalPositions[i];
            targetImages[i].color = normalColor;
        }

        warningCoroutine = null;
    }

    private void ResetIconPositions()
    {
        foreach (Image icon in icons)
        {
            if (icon == null)
                continue;

            // The layout group controls the normal position.
            icon.rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    public void PreviewAPCost(int cost)
    {
        previewAPCost = Mathf.Max(0, cost);
        ApplyAPColors();
    }

    public void ClearAPPreview()
    {
        previewAPCost = 0;
        ApplyAPColors();
    }

    private void ApplyAPColors()
    {
        for (int i = 0; i < icons.Count; i++)
        {
            if (icons[i] == null || !icons[i].gameObject.activeSelf)
                continue;

            icons[i].color = i < previewAPCost
                ? warningPreviewColor
                : normalColor;
        }
    }
}
