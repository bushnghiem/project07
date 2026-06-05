using TMPro;
using UnityEngine;

public class FleetTooltipUI : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text text;

    void Start()
    {
        Hide();
    }

    public void Show(string content)
    {
        panel.SetActive(true);
        text.text = content;
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}