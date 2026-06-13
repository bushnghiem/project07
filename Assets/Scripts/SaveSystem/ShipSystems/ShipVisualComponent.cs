using UnityEngine;

public class ShipVisualComponent : MonoBehaviour
{
    private UnitBase unitBaseComponent;

    private ShipVisualController visualController;

    private void Awake()
    {
        unitBaseComponent = GetComponent<UnitBase>();
        visualController = GetComponent<ShipVisualController>();
    }

    private void Start()
    {
        Initialize(unitBaseComponent);
    }

    public void Initialize(UnitBase unitBase)
    {
        ShipTemplate template = unitBase.Template;

        visualController.ApplyVisuals(
            template.VisualData
        );
    }
}