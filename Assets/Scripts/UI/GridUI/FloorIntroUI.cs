using TMPro;
using UnityEngine;
using System.Collections;

public class FloorIntroUI : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup canvasGroup;
    public TMP_Text titleText;

    [Header("Timing")]
    public float fadeInTime = 0.5f;
    public float holdTime = 2f;
    public float fadeOutTime = 1f;

    [SerializeField] private TMPTerminalTypewriter typewriter;

    private IEnumerator Start()
    {
        var floor = RunManager.Instance.CurrentRun.currentFloorData;

        if (floor.hasShownFloorIntro)
        {
            gameObject.SetActive(false);
            yield break;
        }

        floor.hasShownFloorIntro = true;

        string floorName = string.IsNullOrEmpty(floor.contentProfile.floorName)
            ? $"Floor {floor.floorIndex + 1}"
            : floor.contentProfile.floorName;

        yield return StartCoroutine(PlayIntro());

        gameObject.SetActive(false);
    }

    private IEnumerator PlayIntro()
    {
        canvasGroup.alpha = 0f;

        float t = 0f;

        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(t / fadeInTime);
            yield return null;
        }

        canvasGroup.alpha = 1f;

        var floor = RunManager.Instance.CurrentRun.currentFloorData;

        string floorName = string.IsNullOrEmpty(floor.contentProfile.floorName)
            ? $"Floor {floor.floorIndex + 1}"
            : floor.contentProfile.floorName;

        yield return typewriter.TypeText(
            $"Entering\n<size=120%>{floorName}</size>\n<size=60%>Sector {floor.floorIndex + 1}</size>");

        yield return new WaitForSeconds(holdTime);

        t = 0f;

        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = 1f - Mathf.Clamp01(t / fadeOutTime);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}