using UnityEngine;

public class FleetInputController : MonoBehaviour
{
    [SerializeField] private FleetUI fleetUI;

    void Start()
    {
        fleetUI.Close();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (fleetUI.gameObject.activeInHierarchy)
            {
                fleetUI.Close();
                GridUIManager.Instance.ClearState();
            }
            else
            {
                if (!GridUIManager.Instance.CanOpen(UIState.Fleet))
                    return;

                GridUIManager.Instance.SetState(UIState.Fleet);
                fleetUI.Open();
            }
        }
    }
}