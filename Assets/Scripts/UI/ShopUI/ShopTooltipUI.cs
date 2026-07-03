using TMPro;
using UnityEngine;

public class ShopTooltipUI : MonoBehaviour
{
    public static ShopTooltipUI Instance;

    [Header("UI")]
    public RectTransform panel;
    public TMP_Text titleText;
    public TMP_Text descriptionText;

    [Header("Settings")]
    public Vector2 mouseOffset = new Vector2(20f, -20f);

    private void Awake()
    {
        Instance = this;
        Hide();
    }

    private void Update()
    {
        if (!panel.gameObject.activeSelf)
            return;

        UpdatePosition();
    }

    public void Show(string title, string description)
    {
        titleText.text = title;
        descriptionText.text = description;

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

        // Right side -> show left of cursor
        if (mouse.x + offset.x + size.x > Screen.width)
        {
            pivot.x = 1f;
            offset.x = -mouseOffset.x;
        }

        // Bottom -> show above cursor
        if (mouse.y + offset.y - size.y < 0)
        {
            pivot.y = 0f;
            offset.y = Mathf.Abs(mouseOffset.y);
        }

        panel.pivot = pivot;
        panel.position = mouse + offset;
    }
}