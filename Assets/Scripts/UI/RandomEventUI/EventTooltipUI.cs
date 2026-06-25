using TMPro;
using UnityEngine;

public class EventTooltipUI : MonoBehaviour
{
    public static EventTooltipUI Instance;

    [Header("References")]
    public RectTransform panel;
    public TextMeshProUGUI tooltipText;

    [Header("Settings")]
    public Vector2 mouseOffset = new Vector2(-20f, -20f);

    private Canvas canvas;

    private void Awake()
    {
        Instance = this;

        canvas = GetComponentInParent<Canvas>();

        Hide();
    }

    private void Update()
    {
        if (!panel.gameObject.activeSelf)
            return;

        UpdatePosition();
    }

    public void Show(string text)
    {
        tooltipText.text = text;

        panel.gameObject.SetActive(true);

        Canvas.ForceUpdateCanvases();

        UpdatePosition();
    }

    public void Hide()
    {
        panel.gameObject.SetActive(false);
    }

    private void UpdatePosition()
    {
        Vector2 position = (Vector2)Input.mousePosition + mouseOffset;

        float width = panel.rect.width;
        float height = panel.rect.height;

        // Clamp inside screen bounds
        position.x = Mathf.Clamp(
            position.x,
            0,
            Screen.width - width);

        position.y = Mathf.Clamp(
            position.y,
            height,
            Screen.height);

        panel.position = position;
    }
}