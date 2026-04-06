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
    public EventUI eventUI;
    public Vector3 fakeEnemyAnchorPos; // Here just to be here
    public Vector3 playerAnchorPos; // Set far away from grid
    public FormationData playerFormation; // Set to formation created for grid
    public ShipHolder shipHolder; // See all player ship instances

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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) TryMove(Vector2Int.up);
        if (Input.GetKeyDown(KeyCode.S)) TryMove(Vector2Int.down);
        if (Input.GetKeyDown(KeyCode.A)) TryMove(Vector2Int.left);
        if (Input.GetKeyDown(KeyCode.D)) TryMove(Vector2Int.right);
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
        }
    }

    public void HandleRandomEventTile(TileData tile)
    {
        if (tile.assignedEvent == null)
        {
            Debug.Log("No event assigned.");
            return;
        }
        Debug.Log(tile.assignedEvent);
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
        Debug.Log("Fight: " + tile.assignedEncounter?.encounterName);
        shipHolder.RemovePlayersPassiveEffects();
        SceneManager.LoadScene("SpawnTestScene");
    }

    public void HandlePortalTile()
    {
        StartCoroutine(HandleFloorTransition());
    }

    public void HandleShopTile()
    {
        Vector2Int shopPos = gridPosition;

        shopManager.GenerateShop(shopPos);

        shopUI.PopulateShop();
        shopUI.gameObject.SetActive(true);
    }

    IEnumerator HandleFloorTransition()
    {
        var run = RunManager.Instance.CurrentRun;

        // Insert boss fight here later

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

        var profile = RunManager.Instance.GetProfileForFloor(run.currentFloor);

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
}
