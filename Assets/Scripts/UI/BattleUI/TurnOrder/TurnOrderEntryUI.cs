using UnityEngine;
using UnityEngine.UI;

public class TurnOrderEntryUI : MonoBehaviour
{
    [SerializeField] private Image portraitImage;
    [SerializeField] private GameObject currentTurnHighlight;

    private Unit unit;

    public void Setup(Unit unit, bool isCurrentTurn)
    {
        this.unit = unit;

        SetCurrent(isCurrentTurn);

        // Temporary coloring until portraits are added
        portraitImage.color =
            unit.IsPlayerControllable
            ? Color.cyan
            : Color.red;
    }

    public void SetCurrent(bool value)
    {
        currentTurnHighlight.SetActive(value);
    }

    public Unit GetUnit()
    {
        return unit;
    }
}