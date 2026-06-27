using UnityEngine;
using TMPro;

public class TargetingUI : MonoBehaviour
{
    [SerializeField] ItemTargetingController targeting;

    [SerializeField] GameObject panel;

    [SerializeField] TMP_Text itemName;

    [SerializeField] TMP_Text instructions;

    private void Awake()
    {
        if (targeting == null)
        {
            Debug.LogError("TargetingUI: TargetingController not assigned.");
            enabled = false;
            return;
        }

        targeting.OnTargetingStarted += Show;
        targeting.OnTargetingEnded += Hide;
    }

    private void OnDestroy()
    {
        if (targeting == null)
            return;

        targeting.OnTargetingStarted -= Show;
        targeting.OnTargetingEnded -= Hide;
    }

    private void Start()
    {
        panel.SetActive(false);
    }

    void Show(ActiveItemInstance item)
    {
        panel.SetActive(true);

        itemName.text = item.itemData.itemName;

        instructions.text =
            "Left Click to Confirm\nRight Click to Cancel";
    }

    void Hide()
    {
        panel.SetActive(false);
    }
}
