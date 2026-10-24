using UnityEngine;

public class FloorIntroUI : MonoBehaviour
{
    [Header("Floor Intro")]
    [SerializeField] private Color floorIntroColor = Color.white;
    [SerializeField] private float typingSpeed = 20f;
    [SerializeField] private float holdTime = 2f;

    private void Start()
    {
        var floor = RunManager.Instance.CurrentRun.currentFloorData;

        if (floor.hasShownFloorIntro)
            return;

        floor.hasShownFloorIntro = true;

        string floorName = string.IsNullOrEmpty(
            floor.contentProfile.floorName)
            ? $"Floor {floor.floorIndex + 1}"
            : floor.contentProfile.floorName;

        string message =
            $"Entering\n<size=120%>{floorName}</size>\n" +
            $"<size=60%>Sector {floor.floorIndex + 1}</size>";

        ScreenMessageController.Instance.ShowMessage(
            message,
            floorIntroColor,
            typingSpeed,
            holdTime,
            priority: 10
        );
    }
}
