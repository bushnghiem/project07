using UnityEngine;

public enum UIState
{
    None,
    Grid,
    Event,
    Shop,
    Fleet
}

public class GridUIManager : MonoBehaviour
{
    public static GridUIManager Instance;

    public UIState CurrentState { get; private set; } = UIState.None;

    [Header("UI References")]
    public FleetUI fleetUI;
    public ShopUI shopUI;
    public EventUI eventUI;
    public TileTooltipUI tileTooltip;

    private void Awake()
    {
        Instance = this;
    }

    public bool CanOpen(UIState state)
    {
        return CurrentState == UIState.None || CurrentState == state;
    }

    public void SetState(UIState newState)
    {
        CurrentState = newState;

        // Close conflicting UI
        if (newState != UIState.Fleet && fleetUI != null)
            fleetUI.Close();

        if (newState != UIState.Shop && shopUI != null)
            shopUI.gameObject.SetActive(false);

        if (newState != UIState.Event && eventUI != null)
            eventUI.gameObject.SetActive(false);

        if (newState != UIState.None && tileTooltip != null)
            tileTooltip.Hide();
    }

    public void ClearState()
    {
        CurrentState = UIState.None;
    }
}