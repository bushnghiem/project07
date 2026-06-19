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

        titleText.text = $"Entering\n<size=120%>{floorName}</size>\n<size=60%>Sector {floor.floorIndex + 1}</size>";

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
            canvasGroup.alpha = t / fadeInTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(holdTime);

        t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = 1f - (t / fadeOutTime);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}