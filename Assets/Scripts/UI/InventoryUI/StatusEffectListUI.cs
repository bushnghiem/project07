using System.Collections.Generic;
using UnityEngine;

public class StatusEffectListUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform content;
    [SerializeField] private StatusEffectEntryUI entryPrefab;
    [SerializeField] private FleetTooltipUI tooltip;

    private readonly List<StatusEffectEntryUI> entries = new();

    private StatusEffectController controller;

    public void ShowUnit(UnitBase unit)
    {
        Unsubscribe();

        controller = unit != null
            ? unit.GetComponent<StatusEffectController>()
            : null;

        if (controller != null)
        {
            controller.OnEffectsChanged += Refresh;
        }

        Refresh();
    }

    private void Refresh()
    {
        foreach (var entry in entries)
        {
            if (entry != null)
                Destroy(entry.gameObject);
        }

        entries.Clear();

        if (controller == null)
            return;

        foreach (var effect in controller.ActiveEffects)
        {
            StatusEffectEntryUI entry = Instantiate(
                entryPrefab,
                content
            );

            entry.Init(effect, tooltip);
            entries.Add(entry);
        }
    }

    private void Unsubscribe()
    {
        if (controller != null)
        {
            controller.OnEffectsChanged -= Refresh;
        }

        controller = null;
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
}