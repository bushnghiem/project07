using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GridMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector2Int gridPosition;
    public GridManager gridManager;
    public UnitSpawner unitSpawner;
    public ShopManager shopManager;
    public ShopUI shopUI;
    public ChestUI chestUI;
    public EventUI eventUI;
    public Vector3 fakeEnemyAnchorPos; // Here just to be here
    public Vector3 playerAnchorPos; // Set far away from grid
    public FormationData playerFormation; // Set to formation created for grid
    public ShipHolder shipHolder; // See all player ship instances
    public bool inputLocked;

    IEnumerator Start()
    {
        var run = RunManager.Instance.CurrentRun;
        var floor = run.currentFloorData;

        // Wait until grid is ready
        yield return new WaitUntil(() => gridManager.IsGridReady);

        gridPosition = floor.currentGridPosition;
        transform.position = gridManager.GetWorldPosition(gridPosition.x, gridPosition.y);

        unitSpawner.SetAnchorPositions(playerAnchorPos, fakeEnemyAnchorPos);
        unitSpawner.SpawnPlayerTeam(run.team, playerFormation);

        CorruptionManager.Instance.Init(gridManager);

        TileData tile = gridManager.grid[gridPosition.x, gridPosition.y];
        HandleTileEvent(tile);

        if (!string.IsNullOrEmpty(floor.contentProfile.floorName))
            Debug.Log($"Entering: {floor.contentProfile.floorName}");
        else
            Debug.Log($"Entering Floor {floor.floorIndex + 1}");
        CheckPendingReward();
    }

    void Update()
    {
        if (inputLocked)
            return;

        if (Input.GetKeyDown(KeyCode.W)) TryMove(Vector2Int.up);
        if (Input.GetKeyDown(KeyCode.S)) TryMove(Vector2Int.down);
        if (Input.GetKeyDown(KeyCode.A)) TryMove(Vector2Int.left);
        if (Input.GetKeyDown(KeyCode.D)) TryMove(Vector2Int.right);

        if (Input.GetKeyDown(KeyCode.Space))
            InteractWithCurrentTile();
    }

    void TryMove(Vector2Int direction)
    {
        Vector2Int targetPos = gridPosition + direction;

        if (!gridManager.IsInsideGrid(targetPos.x, targetPos.y))
            return;

        TileData tile = gridManager.grid[targetPos.x, targetPos.y];

        if (tile != null && tile.IsWalkable)
        {
            var floor = RunManager.Instance.CurrentRun.currentFloorData;

            AddTime(1);

            gridPosition = targetPos;
            transform.position = gridManager.GetWorldPosition(gridPosition.x, gridPosition.y);

            floor.currentGridPosition = gridPosition;

            SaveManager.Instance.SaveRun();

            HandleTileEvent(tile);
        }
    }

    void AddTime(int amount)
    {
        var floor = RunManager.Instance.CurrentRun.currentFloorData;

        floor.timeElapsed += amount;

        CorruptionManager.Instance.OnTimePassed();

        SaveManager.Instance.SaveRun();
    }

    void HandleTileEvent(TileData tile)
    {
        switch (tile.tileType)
        {
            case TileType.Event:
                HandleRandomEventTile(tile);
                break;
            case TileType.Empty:
                HandleEmptyTile();
                break;
            case TileType.Combat:
                HandleCombatTile(tile);
                break;
            case TileType.Portal:
                HandlePortalTile();
                break;
            case TileType.Shop:
                HandleShopTile();
                break;
            case TileType.Chest:
                HandleChestTile();
                break;
        }
    }

    public void HandleRandomEventTile(TileData tile)
    {
        if (tile.assignedEvent == null)
            return;

        var floor = RunManager.Instance.CurrentRun.currentFloorData;

        if (!floor.discoveredEventTiles.Contains(gridPosition))
        {
            floor.discoveredEventTiles.Add(gridPosition);
            SaveManager.Instance.SaveRun();
        }

        GridUIManager.Instance.SetState(UIState.Event);

        inputLocked = true;
        eventUI.ShowEvent(tile.assignedEvent);
    }

    public void HandleEmptyTile()
    {
        Debug.Log("Nothing");
    }

    public void HandleCombatTile(TileData tile)
    {
        var floor = RunManager.Instance.CurrentRun.currentFloorData;

        floor.currentGridPosition = gridPosition;
        floor.currentEncounter = tile.assignedEncounter;
        floor.currentEncounterIsCorrupted = tile.isCorrupted;

        if (tile.isElite)
        {
            floor.pendingReward = PendingRewardType.Elite;
        }

        Debug.Log("Fight: " + tile.assignedEncounter?.encounterName);

        shipHolder.RemovePlayersPassiveEffects();

        SceneManager.LoadScene("SpawnTestScene");
    }

    public void HandlePortalTile()
    {
        var floor = RunManager.Instance.CurrentRun.currentFloorData;

        // Boss floor and boss not defeated yet
        if (IsBossFloor() && !floor.bossDefeated)
        {
            floor.pendingReward = PendingRewardType.Boss;
            StartBossFight();
            return;
        }
        else
        {
            if (floor.pendingReward != PendingRewardType.Boss)
            {
                StartCoroutine(HandleFloorTransition());
            }
        }
    }

    public void HandleShopTile()
    {
        Vector2Int shopPos = gridPosition;

        shopManager.GenerateShop(shopPos);

        GridUIManager.Instance.SetState(UIState.Shop);

        inputLocked = true;

        shopUI.PopulateShop();
        shopUI.gameObject.SetActive(true);
    }

    public void HandleChestTile()
    {
        var floor = RunManager.Instance.CurrentRun.currentFloorData;

        ChestData chest =
            floor.chests.Find(c => c.gridPosition == gridPosition);

        if (chest != null && chest.opened)
            return;

        GridUIManager.Instance.OpenChest();

        inputLocked = true;
    }

    IEnumerator HandleFloorTransition()
    {
        var run = RunManager.Instance.CurrentRun;


        // Save completed floor
        run.completedFloors.Add(run.currentFloorData);

        run.currentFloor++;

        GenerateNextFloor(run);

        SaveManager.Instance.SaveRun();

        SceneManager.LoadScene("TestGrid");
        yield break;
    }

    void GenerateNextFloor(RunData run)
    {
        int newSeed = run.runSeed + run.currentFloor * 1000;

        var profile = RunManager.Instance.GetProfileForFloor(run.currentFloor, run.runSeed);

        run.currentFloorData = new FloorData
        {
            floorIndex = run.currentFloor,
            floorSeed = newSeed,
            contentProfile = profile,

            currentGridPosition = Vector2Int.zero,
            timeElapsed = 0,
            nextCorruptionTimeThreshold = 5,
            corruptionRadius = -1
        };
    }

    bool IsBossFloor()
    {
        var run = RunManager.Instance.CurrentRun;

        // Floors 3,6,9...
        return (run.currentFloor + 1) % 3 == 0;
    }

    void StartBossFight()
    {
        var floor = RunManager.Instance.CurrentRun.currentFloorData;

        floor.currentEncounter = floor.contentProfile.bossEncounter;

        Debug.Log("Boss Fight: " + floor.currentEncounter?.encounterName);

        shipHolder.RemovePlayersPassiveEffects();

        SaveManager.Instance.SaveRun();

        SceneManager.LoadScene("SpawnTestScene");
    }

    void InteractWithCurrentTile()
    {
        TileData tile =
            gridManager.grid[gridPosition.x, gridPosition.y];

        if (tile == null)
            return;

        HandleTileEvent(tile);
    }

    void CheckPendingReward()
    {
        var floor = RunManager.Instance.CurrentRun.currentFloorData;

        switch (floor.pendingReward)
        {
            case PendingRewardType.Elite:

                RewardMenuUI.Instance.Show(
                    RewardGenerator.Generate(
                        floor.contentProfile.eliteRewards,
                        1),
                    () =>
                    {
                        floor.pendingReward = PendingRewardType.None;
                        SaveManager.Instance.SaveRun();
                    });

                break;

            case PendingRewardType.Boss:

                RewardMenuUI.Instance.Show(
                    RewardGenerator.Generate(
                        floor.contentProfile.bossRewards,
                        3),
                    () =>
                    {
                        floor.pendingReward = PendingRewardType.None;

                        StartCoroutine(HandleFloorTransition());
                    });

                break;
        }
    }
}
