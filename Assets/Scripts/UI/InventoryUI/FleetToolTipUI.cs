using TMPro;
using UnityEngine;

public class FleetTooltipUI : MonoBehaviour
{
    public GameObject panel;

    [Header("UI")]
    public TMP_Text titleText;
    public TMP_Text descriptionText;

    private void Start()
    {
        Hide();
    }

    public void Show(string title, string description)
    {
        panel.SetActive(true);

        titleText.text = title;
        descriptionText.text = description;
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}