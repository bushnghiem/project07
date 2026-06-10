using UnityEngine;
using UnityEngine.UI;

public class CombatLogUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CombatLogRow rowPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private ScrollRect scrollRect;

    [Header("Settings")]
    [SerializeField] private int maxVisibleEntries = 100;

    private void Start()
    {
        CombatLogManager.Instance.OnLogAdded += HandleLogAdded;

        foreach (var entry in CombatLogManager.Instance.Entries)
        {
            CreateRow(entry);
        }
    }

    private void OnDestroy()
    {
        if (CombatLogManager.Instance != null)
        {
            CombatLogManager.Instance.OnLogAdded -= HandleLogAdded;
        }
    }

    private void HandleLogAdded(CombatLogEntry entry)
    {
        CreateRow(entry);

        TrimOldEntries();

        Canvas.ForceUpdateCanvases();

        scrollRect.verticalNormalizedPosition = 0f;
    }

    private void CreateRow(CombatLogEntry entry)
    {
        CombatLogRow row =
            Instantiate(rowPrefab, contentParent);

        row.Initialize(entry);
    }

    private void TrimOldEntries()
    {
        while (contentParent.childCount > maxVisibleEntries)
        {
            Destroy(contentParent.GetChild(0).gameObject);
        }
    }
}