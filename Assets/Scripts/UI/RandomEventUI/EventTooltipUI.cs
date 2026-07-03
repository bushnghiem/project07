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
        Canvas.ForceUpdateCanvases();

        Vector2 mouse = Input.mousePosition;
        Vector2 size = panel.rect.size;

        Vector2 pivot = new Vector2(0f, 1f);
        Vector2 offset = mouseOffset;

        // If there's not enough room on the right, show it to the left.
        if (mouse.x + offset.x + size.x > Screen.width)
        {
            pivot.x = 1f;
            offset.x = -Mathf.Abs(mouseOffset.x);
        }

        // If there's not enough room below, show it above.
        if (mouse.y + offset.y - size.y < 0)
        {
            pivot.y = 0f;
            offset.y = Mathf.Abs(mouseOffset.y);
        }

        panel.pivot = pivot;
        panel.position = mouse + offset;
    }
}