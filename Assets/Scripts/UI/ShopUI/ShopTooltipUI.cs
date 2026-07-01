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
        Vector2 position = (Vector2)Input.mousePosition + mouseOffset;

        if (position.x < 0) position.x = 0;
        if (position.y < 0) position.y = 0;

        panel.position = position;
    }
}