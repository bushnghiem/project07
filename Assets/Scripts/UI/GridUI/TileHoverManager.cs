using UnityEngine;

public class TileHoverManager : MonoBehaviour
{
    public Camera worldCamera;
    public GridManager gridManager;
    public PlayerSelectionUI playerSelectUI;

    TileHover currentHover;

    void Update()
    {
        // Another UI is open.
        // Stop tile detection only.
        if (GridUIManager.Instance != null &&
            GridUIManager.Instance.CurrentState != UIState.None)
        {
            currentHover = null;
            return;
        }

        if (playerSelectUI.Active)
        {
            currentHover = null;
            return;
        }

        Ray ray = worldCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            TileHover hover = hit.collider.GetComponent<TileHover>();

            if (hover != null)
            {
                if (hover != currentHover)
                {
                    currentHover = hover;

                    TooltipUI.Instance.Show(
                        TileTooltipBuilder.Build(
                            hover.tile,
                            hover.gridPosition,
                            gridManager));
                }

                return;
            }
        }

        if (currentHover != null)
        {
            currentHover = null;
            TooltipUI.Instance.Hide();
        }
    }
}