using TMPro;
using UnityEngine;

public class TileTooltipUI : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text description;

    public GridManager gridManager;
    public RectTransform panel;

    [Header("Settings")]
    public Vector2 mouseOffset = new Vector2(-20f, -20f);

    void Update()
    {
        if (!panel.gameObject.activeSelf)
            return;

        if (GridUIManager.Instance != null &&
            GridUIManager.Instance.CurrentState != UIState.None)
            return;

        UpdatePosition();
    }

    public void Show(TileData tile, Vector2Int position)
    {
        if (GridUIManager.Instance != null &&
            GridUIManager.Instance.CurrentState != UIState.None)
            return;

        panel.gameObject.SetActive(true);

        switch (tile.tileType)
        {
            case TileType.Combat:

                if (tile.isCorrupted)
                {
                    title.text = "Corrupted Encounter";
                    description.text = "Corrupted threat detected.";
                }
                else if (tile.isElite)
                {
                    title.text = "Elite Encounter";
                    description.text = "A powerful threat detected.";
                }
                else
                {
                    title.text = "Combat";
                    description.text = "Threats detected.";
                }

                break;

            case TileType.Event:

                if (gridManager.IsEventDiscovered(position))
                {
                    title.text = "Known Event";
                    description.text = tile.assignedEvent.eventName;
                }
                else
                {
                    title.text = "Unknown Event";
                    description.text = "An unexplored Event.";
                }

                break;

            case TileType.Shop:
                title.text = "Shop";
                description.text = "Purchase equipment.";
                break;

            case TileType.Portal:

                var run = RunManager.Instance.CurrentRun;
                bool isBoss = (run.currentFloor + 1) % 3 == 0;

                if (isBoss)
                {
                    title.text = "Boss Gate";
                    description.text = "A massive threat awaits beyond this portal.";
                }
                else
                {
                    title.text = "Portal";
                    description.text = "Travel to the next sector.";
                }

                break;

            default:
                title.text = "";
                description.text = "";
                break;
        }
    }

    public void Hide()
    {
        panel.gameObject.SetActive(false);
    }

    private void UpdatePosition()
    {
        Vector2 position = (Vector2)Input.mousePosition + mouseOffset;

        float width = panel.rect.width;
        float height = panel.rect.height;

        position.x = Mathf.Clamp(position.x, 0, Screen.width - width);
        position.y = Mathf.Clamp(position.y, height, Screen.height);

        panel.position = position;
    }
}