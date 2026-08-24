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
        UnitBase unitBase = unit as UnitBase;
        SetCurrent(isCurrentTurn);
        portraitImage.sprite = unitBase.Template.VisualData.shipIcon;
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