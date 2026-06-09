using TMPro;
using UnityEngine;

public class WorldTooltipUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panel;
    public CanvasGroup canvasGroup;

    [Header("Fade")]
    public float fadeInSpeed = 10f;
    public float fadeOutSpeed = 10f;

    [Header("Position")]
    public Vector2 offset = new(-50, -25);

    [Header("Text")]
    public TMP_Text nameText;
    public TMP_Text healthText;
    public TMP_Text shieldText;
    public TMP_Text collisionText;
    public TMP_Text extraText;

    private bool shouldShow;

    void Awake()
    {
        canvasGroup.alpha = 0f;
        panel.SetActive(true);
    }

    void Update()
    {
        if (panel.activeSelf)
        {
            panel.transform.position =
                Input.mousePosition + (Vector3)offset;
        }

        float targetAlpha =
            shouldShow ? 1f : 0f;

        float speed =
            shouldShow ? fadeInSpeed : fadeOutSpeed;

        canvasGroup.alpha = Mathf.MoveTowards(
            canvasGroup.alpha,
            targetAlpha,
            speed * Time.deltaTime
        );
    }

    public void Show(InspectionData data)
    {
        shouldShow = true;

        nameText.text = data.Name;

        healthText.text =
            $"HP {data.CurrentHP:0}/{data.MaxHP:0}";

        shieldText.text =
            $"Shield {data.Shield}";

        collisionText.text =
            $"Collision Damage {data.CollisionDamage:0}";

        extraText.text =
            data.ExtraInfo;
    }

    public void Hide()
    {
        shouldShow = false;
    }
}