using TMPro;
using UnityEngine;

public class FleetTooltipUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [Header("UI")]
    [SerializeField] private TMP_Text titleText;

    [SerializeField] private TMPTerminalTypewriter typewriter;

    private void Start()
    {
        Hide();
    }

    public void Show(string title, string description)
    {
        panel.SetActive(true);

        titleText.text = title;

        typewriter.ShowText(description);
    }

    public void Hide()
    {
        typewriter.StopTyping();
        panel.SetActive(false);
    }

}
