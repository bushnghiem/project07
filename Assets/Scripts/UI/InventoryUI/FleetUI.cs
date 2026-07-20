using TMPro;
using UnityEngine;

public class FleetUI : AnimatedUI
{
    [Header("References")]
    public ShipHolder shipHolder;

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
    public TMP_Text chargesText;

    private Player selectedPlayer;

    public bool activated = false;



    private void OnEnable()
    {
        PopulateShipList();
    }

    public override void Open()
    {
        base.Open();
        RefreshFleet();
        activated = true;
    }

    public override void Close()
    {
        base.Close();
    }

    protected override void PlayOpenAnimation()
    {
        Debug.Log("Open Anim");
        animator.SetTrigger("Open");
    }

    protected override void PlayCloseAnimation()
    {
        animator.SetTrigger("Close");
    }

    public void DisableUI()
    {
        gameObject.SetActive(false);
    }

    public void RefreshFleet()
    {
        if (shipHolder == null || shipHolder.allPlayers == null)
            return;

        PopulateShipList();
    }

    public void PopulateShipList()
    {
        foreach (Transform child in shipListParent)
        {
            Destroy(child.gameObject);
        }

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

    private void PopulateStats(Player player)
    {
        foreach (Transform child in statsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (ShipStatType stat in System.Enum.GetValues(typeof(ShipStatType)))
        {
            float value = player.GetStat(stat);

            GameObject entry =
                Instantiate(
                    statPrefab,
                    statsParent
                );

            entry.GetComponent<StatEntryUI>()
                .Init(
                    stat.ToString(),
                    value,
                    tooltipUI
                );
        }
    }

    private void PopulateItems(Player player)
    {
        Clear(activeParent);
        Clear(projectileParent);
        Clear(passiveParent);

        if (player.ActiveItem != null)
        {
            CreateItem(
                player.ActiveItem.itemData,
                activeParent
            );
        }

        if (player.Projectile != null)
        {
            CreateItem(
                player.ProjectileItem,
                projectileParent
            );
        }

        foreach (var passive in player.PassiveItems)
        {
            CreateItem(
                passive.itemData,
                passiveParent
            );
        }
    }

    private void CreateItem(Item item, Transform parent)
    {
        GameObject obj =
            Instantiate(
                itemEntryPrefab,
                parent
            );

        obj.GetComponent<ItemEntryUI>()
            .Init(item, tooltipUI);
    }

    public void SelectPlayer(Player player)
    {
        selectedPlayer = player;

        RefreshDetails();
    }

    private void Clear(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    private void RefreshDetails()
    {
        if (selectedPlayer == null)
            return;

        nameText.text =
            selectedPlayer.RunData.uniqueID;

        healthText.text =
            $"HP: {selectedPlayer.CurrentHealth}";

        chargesText.text =
            $"Charges: {selectedPlayer.CurrentCharges}";

        PopulateStats(selectedPlayer);
        PopulateItems(selectedPlayer);
    }
}