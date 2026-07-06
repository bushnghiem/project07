using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestUI : MonoBehaviour
{
    [Header("References")]
    public ItemDatabase itemDatabase;
    public ShipHolder shipHolder;

    [Header("Panels")]
    public GameObject openPanel;
    public GameObject rewardsPanel;

    [Header("Buttons")]
    public Button openButton;
    public Button leaveButton;

    [Header("Reward Slots")]
    public Transform rewardParent;
    public ItemChoiceSlot rewardSlotPrefab;

    [Header("UI")]
    public TMP_Text keyText;

    private List<Item> rewards = new();

    private void Awake()
    {
        gameObject.SetActive(false);

        openButton.onClick.AddListener(OpenChest);
        leaveButton.onClick.AddListener(CloseChest);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        openPanel.SetActive(true);
        rewardsPanel.SetActive(false);

        RefreshKeys();
    }

    void RefreshKeys()
    {
        int keys = RunManager.Instance.CurrentRun.runKeys;

        keyText.text = $"Keys: {keys}";

        openButton.interactable = keys > 0;
    }

    void OpenChest()
    {
        if (!RewardManager.Instance.SpendRunKeys(1))
            return;

        GenerateRewards();

        openPanel.SetActive(false);
        rewardsPanel.SetActive(true);

        PopulateRewards();
    }

    void GenerateRewards()
    {
        rewards.Clear();

        var run = RunManager.Instance.CurrentRun;
        var floor = run.currentFloorData;
        Vector2Int pos = floor.currentGridPosition;

        ChestData chest =
            floor.chests.Find(c => c.gridPosition == pos);

        if (chest == null)
        {
            chest = new ChestData
            {
                gridPosition = pos
            };

            floor.chests.Add(chest);
        }

        int seed =
            run.runSeed ^
            (pos.x * 73856093) ^
            (pos.y * 19349663);

        System.Random rng = new System.Random(seed);

        var allItems = floor.contentProfile.shopItems;

        var passives =
            allItems.Where(i => i.slotType == ItemSlotType.Passive).ToList();

        var actives =
            allItems.Where(i => i.slotType == ItemSlotType.Active).ToList();

        var projectiles =
            allItems.Where(i => i.slotType == ItemSlotType.Projectile).ToList();

        if (chest.rewardItemIDs.Count == 0)
        {
            chest.rewardItemIDs.Add(GetRandom(passives, rng).itemID);
            chest.rewardItemIDs.Add(GetRandom(actives, rng).itemID);
            chest.rewardItemIDs.Add(GetRandom(projectiles, rng).itemID);

            SaveManager.Instance.SaveRun();
        }

        foreach (string id in chest.rewardItemIDs)
        {
            rewards.Add(itemDatabase.GetItem(id));
        }
    }

    Item GetRandom(List<Item> pool, System.Random rng)
    {
        return pool[rng.Next(pool.Count)];
    }

    void PopulateRewards()
    {
        foreach (Transform child in rewardParent)
            Destroy(child.gameObject);

        foreach (var item in rewards)
        {
            ItemChoiceSlot slot =
                Instantiate(rewardSlotPrefab, rewardParent);

            slot.Setup(item, GiveItem);
        }
    }

    void GiveItem(Item item)
    {
        PlayerSelectionUI.Instance.Open(
            shipHolder.allPlayers,
            player =>
            {
                RewardManager.Instance.AddItemToPlayer(player, item);

                var floor = RunManager.Instance.CurrentRun.currentFloorData;

                ChestData chest =
                    floor.chests.Find(c => c.gridPosition == floor.currentGridPosition);

                if (chest != null)
                {
                    chest.opened = true;
                    GridManager grid = FindFirstObjectByType<GridManager>();

                    grid.grid[floor.currentGridPosition.x,
                              floor.currentGridPosition.y].tileType = TileType.Empty;

                    grid.ClearTileVisualAt(
                        floor.currentGridPosition.x,
                        floor.currentGridPosition.y);
                }

                SaveManager.Instance.SaveRun();

                CloseChest();
            });
    }

    public void CloseChest()
    {
        gameObject.SetActive(false);

        GridUIManager.Instance.ClearState();

        FindFirstObjectByType<GridMovement>().inputLocked = false;
    }
}