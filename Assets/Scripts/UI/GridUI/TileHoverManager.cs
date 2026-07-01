using UnityEngine;

public class TileHoverManager : MonoBehaviour
{
    public Camera worldCamera;

    public TileTooltipUI tooltip;

    TileHover currentHover;

    void Update()
    {
        Ray ray = worldCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            TileHover hover = hit.collider.GetComponent<TileHover>();

            if (hover != null)
            {
                if (hover != currentHover)
                {
                    currentHover = hover;

                    tooltip.Show(
                        hover.tile,
                        hover.gridPosition);
                }

                return;
            }
        }

        currentHover = null;
        tooltip.Hide();
    }
}