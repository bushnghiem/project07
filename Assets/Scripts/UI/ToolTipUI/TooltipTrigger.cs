using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    private Func<TooltipData> tooltipProvider;

    public void SetProvider(Func<TooltipData> provider)
    {
        tooltipProvider = provider;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipProvider != null)
            TooltipUI.Instance.Show(tooltipProvider());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.Hide();
    }

    private void OnDisable()
    {
        if (TooltipUI.Instance != null)
            TooltipUI.Instance.Hide();
    }
}