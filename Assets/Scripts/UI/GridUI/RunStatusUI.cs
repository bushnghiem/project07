using TMPro;
using UnityEngine;

public class RunStatusUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panel;
    public CanvasGroup canvasGroup;

    [Header("Fade")]
    public float fadeInSpeed = 20f;
    public float fadeOutSpeed = 8f;

    private bool isShowing;

    [Header("Texts")]
    public TMP_Text floorInfoText;
    public TMP_Text currencyText;
    public TMP_Text timeText;
    public TMP_Text corruptionText;
    public TMP_Text bossText;
    public TMP_Text objectiveText;

    void Start()
    {
        canvasGroup.alpha = 0f;
        panel.SetActive(true);
        Refresh();
    }

    void Update()
    {
        bool show = Input.GetKey(KeyCode.LeftShift);

        if (show)
            Refresh();

        float targetAlpha = show ? 1f : 0f;

        float speed = show ? fadeInSpeed : fadeOutSpeed;

        canvasGroup.alpha = Mathf.MoveTowards(
            canvasGroup.alpha,
            targetAlpha,
            speed * Time.deltaTime
        );
    }

    void Refresh()
    {
        var run = RunManager.Instance.CurrentRun;
        var floor = run.currentFloorData;

        floorInfoText.text = $"Sector {run.currentFloor + 1} // {floor.contentProfile.floorName}";

        currencyText.text = $"Units: {run.runCurrency}";

        timeText.text = $"Time Elapsed: {floor.timeElapsed}";

        corruptionText.text = $"Corruption Grows in {floor.TurnsUntilCorruption} Moves";

        bool bossFloor = (run.currentFloor + 1) % 3 == 0;

        bossText.text = bossFloor ? "Boss Sector" : "Normal Sector";

        objectiveText.text =
            bossFloor
            ? "Defeat the Boss"
            : "Reach the Portal";
    }
}