using UnityEngine;
using TMPro;

public class FlingTargetingUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private TMP_Text actionName;

    [SerializeField]
    private TMP_Text instructions;

    private void Awake()
    {
        FlingEvent.OnFlingTargetingStarted += Show;
        FlingEvent.OnFlingTargetingEnded += Hide;
    }

    private void OnDestroy()
    {
        FlingEvent.OnFlingTargetingStarted -= Show;
        FlingEvent.OnFlingTargetingEnded -= Hide;
    }

    private void Start()
    {
        panel.SetActive(false);
    }

    private void Show(ActionType actionType)
    {
        panel.SetActive(true);
        Debug.Log("Show panel");

        switch (actionType)
        {
            case ActionType.Move:
                actionName.text = "Move";
                instructions.text =
                    "Left Click and drag your ship to aim\n" +
                    "Release to Confirm Movement\n" +
                    "Right Click to Cancel";
                break;

            case ActionType.Shoot:
                actionName.text = "Shoot";
                instructions.text =
                    "Left Click and drag your ship to aim\n" +
                    "Release to Confirm Shot\n" +
                    "Right Click to Cancel";
                break;

            default:
                actionName.text = actionType.ToString();
                instructions.text =
                    "Left Click to Confirm\n" +
                    "Right Click to Cancel";
                break;
        }
    }

    private void Hide()
    {
        panel.SetActive(false);
    }
}
