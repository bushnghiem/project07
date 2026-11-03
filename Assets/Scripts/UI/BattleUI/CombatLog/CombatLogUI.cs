using System.Collections.Generic;
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

    private readonly Queue<CombatLogRow> availableRows = new();
    private readonly List<CombatLogRow> activeRows = new();
    private readonly Queue<CombatLogEntry> pendingEntries = new();

    private void Start()
    {
        if (CombatLogManager.Instance == null)
        {
            Debug.LogError("CombatLogManager instance not found!");
            enabled = false;
            return;
        }

        CombatLogManager.Instance.OnLogAdded += HandleLogAdded;

        // Display existing combat history.
        foreach (CombatLogEntry entry in CombatLogManager.Instance.Entries)
        {
            CreateRow(entry);
            TrimOldEntries();
        }

        ScrollToBottom();
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
        // Don't update the UI immediately.
        pendingEntries.Enqueue(entry);
    }

    private void LateUpdate()
    {
        if (pendingEntries.Count == 0)
            return;

        // Process all incoming messages together.
        while (pendingEntries.Count > 0)
        {
            CombatLogEntry entry = pendingEntries.Dequeue();

            CreateRow(entry);

            TrimOldEntries();
        }

        ScrollToBottom();
    }

    private void CreateRow(CombatLogEntry entry)
    {
        CombatLogRow row;

        if (availableRows.Count > 0)
        {
            // Reuse an existing row.
            row = availableRows.Dequeue();

            row.transform.SetAsLastSibling();
            row.gameObject.SetActive(true);
        }
        else
        {
            // Create a row only when the pool is empty.
            row = Instantiate(rowPrefab, contentParent);
        }

        row.Initialize(entry);

        activeRows.Add(row);
    }

    private void TrimOldEntries()
    {
        while (activeRows.Count > maxVisibleEntries)
        {
            CombatLogRow oldestRow = activeRows[0];

            activeRows.RemoveAt(0);

            // Reset and return the row to the pool.
            oldestRow.ResetRow();
            oldestRow.gameObject.SetActive(false);

            availableRows.Enqueue(oldestRow);
        }
    }

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();

        scrollRect.verticalNormalizedPosition = 0f;
    }
}