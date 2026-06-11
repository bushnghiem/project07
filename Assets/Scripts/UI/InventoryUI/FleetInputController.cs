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
            }
            else
            {
                fleetUI.Open();
            }
        }
    }
}
