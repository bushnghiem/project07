using TMPro;
using UnityEngine;

public class FleetUI : MonoBehaviour
{
    [Header("References")]
    public ShipHolder shipHolder;

    [Header("Panel")]
    public GameObject panel;

    [Header("Ship List")]
    public Transform shipListParent;
    public GameObject shipEntryPrefab;

    [Header("Details")]
    public TMP_Text nameText;
    public TMP_Text healthText;
    public TMP_Text activeText;
    public TMP_Text projectileText;
    public TMP_Text passiveText;

    private Player selectedPlayer;

    void Start()
    {
        panel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            panel.SetActive(!panel.activeSelf);

            if (panel.activeSelf)
            {
                PopulateShipList();
            }
        }
    }

    public void PopulateShipList()
    {
        foreach (Transform child in shipListParent)
            Destroy(child.gameObject);

        foreach (Player player in shipHolder.allPlayers)
        {
            GameObject entry =
                Instantiate(
                    shipEntryPrefab,
                    shipListParent
                );

            entry
                .GetComponent<ShipEntryUI>()
                .Initialize(player, this);
        }

        if (shipHolder.allPlayers.Count > 0)
        {
            SelectPlayer(shipHolder.allPlayers[0]);
        }
    }

    public void SelectPlayer(Player player)
    {
        selectedPlayer = player;

        RefreshDetails();
    }

    void RefreshDetails()
    {
        if (selectedPlayer == null)
            return;

        nameText.text = selectedPlayer.RunData.uniqueID;

        healthText.text =
            $"HP: {selectedPlayer.CurrentHealth}";

        activeText.text =
            selectedPlayer.ActiveItem != null
            ? selectedPlayer.ActiveItem.itemData.itemName
            : "None";

        projectileText.text =
            selectedPlayer.Projectile != null
            ? selectedPlayer.Projectile.projectileName
            : "None";

        passiveText.text = "";

        foreach (var passive in selectedPlayer.PassiveItems)
        {
            passiveText.text +=
                passive.itemData.itemName + "\n";
        }
    }
}