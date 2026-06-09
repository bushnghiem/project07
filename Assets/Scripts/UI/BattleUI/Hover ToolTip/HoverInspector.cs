using UnityEngine;
using UnityEngine.EventSystems;

public class HoverInspector : MonoBehaviour
{
    [SerializeField]
    private Camera worldCamera;

    [SerializeField]
    private WorldTooltipUI tooltip;

    private IInspectable currentInspectable;

    void Update()
    {

        bool inspectionMode =
            Input.GetKey(KeyCode.LeftShift);

        if (!inspectionMode)
        {
            currentInspectable = null;
            tooltip.Hide();
            return;
        }

        Ray ray =
            worldCamera.ScreenPointToRay(
                Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Hit: " + hit.collider.name);
            IInspectable inspectable =
                hit.collider.GetComponentInParent<IInspectable>();

            if (inspectable != null)
            {
                currentInspectable = inspectable;

                tooltip.Show(
                    inspectable.GetInspectionData()
                );

                return;
            }
        }

        currentInspectable = null;
        tooltip.Hide();
    }
}