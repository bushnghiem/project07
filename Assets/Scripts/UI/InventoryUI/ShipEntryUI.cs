using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipEntryUI : MonoBehaviour
{
    public TMP_Text label;
    public Button button;

    private Player player;
    private FleetUI fleetUI;

    public void Initialize(Player p, FleetUI ui)
    {
        player = p;
        fleetUI = ui;

        label.text =
            $"{p.RunData.uniqueID} ({Mathf.RoundToInt(p.CurrentHealth)} HP)";

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            fleetUI.SelectPlayer(player);
        });
    }
}