using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    [Header("UI")]
    public RectTransform panel;
    public TMP_Text titleText;
    public TMP_Text descriptionText;

    [Header("Settings")]
    public Vector2 mouseOffset = new(-20f, -20f);

    void Awake()
    {
        Instance = this;
        Hide();
    }

    void Update()
    {
        if (!panel.gameObject.activeSelf)
            return;

        UpdatePosition();
    }

    public void Show(TooltipData data)
    {
        titleText.text = data.Title;
        descriptionText.text = data.Description;

        panel.gameObject.SetActive(true);

        // Always draw above every other UI
        panel.SetAsLastSibling();

        Canvas.ForceUpdateCanvases();

        UpdatePosition();
    }

    public void Show(string title, string description)
    {
        Show(new TooltipData(title, description));
    }

    public void Hide()
    {
        panel.gameObject.SetActive(false);
    }

    void UpdatePosition()
    {
        Canvas.ForceUpdateCanvases();

        Vector2 mouse = Input.mousePosition;
        Vector2 size = panel.rect.size;

        Vector2 pivot = new(0f, 1f);
        Vector2 offset = mouseOffset;

        if (mouse.x + offset.x + size.x > Screen.width)
        {
            pivot.x = 1f;
            offset.x = -Mathf.Abs(mouseOffset.x);
        }

        if (mouse.y + offset.y - size.y < 0)
        {
            pivot.y = 0f;
            offset.y = Mathf.Abs(mouseOffset.y);
        }

        panel.pivot = pivot;
        panel.position = mouse + offset;
    }
}