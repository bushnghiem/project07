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

    public Transform statsParent;
    public GameObject statPrefab;
    public FleetTooltipUI tooltipUI;

    public Transform activeParent;
    public Transform projectileParent;
    public Transform passiveParent;

    public GameObject itemEntryPrefab;

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

    void PopulateStats(Player player)
    {
        foreach (Transform child in statsParent)
            Destroy(child.gameObject);

        foreach (ShipStatType stat in System.Enum.GetValues(typeof(ShipStatType)))
        {
            float value = player.GetStat(stat);

            GameObject entry = Instantiate(statPrefab, statsParent);

            entry.GetComponent<StatEntryUI>()
                .Init(stat.ToString(), value, tooltipUI);
        }
    }

    void PopulateItems(Player player)
    {
        Clear(activeParent);
        Clear(projectileParent);
        Clear(passiveParent);

        // ACTIVE
        if (player.ActiveItem != null)
        {
            CreateItem(player.ActiveItem.itemData, activeParent);
        }

        // PROJECTILE
        if (player.Projectile != null)
        {
            CreateItem(player.ProjectileItem, projectileParent);
        }

        // PASSIVES
        foreach (var passive in player.PassiveItems)
        {
            CreateItem(passive.itemData, passiveParent);
        }
    }

    void CreateItem(Item item, Transform parent)
    {
        GameObject obj = Instantiate(itemEntryPrefab, parent);

        obj.GetComponent<ItemEntryUI>()
            .Init(item, tooltipUI);
    }

    public void SelectPlayer(Player player)
    {
        selectedPlayer = player;

        RefreshDetails();
    }

    void Clear(Transform parent)
    {
        foreach (Transform child in parent)
            Destroy(child.gameObject);
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

        PopulateStats(selectedPlayer);
        PopulateItems(selectedPlayer);
    }


}