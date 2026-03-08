using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GridMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector2Int gridPosition;
    public GridManager gridManager;
    public UnitSpawner unitSpawner;
    public Vector3 fakeEnemyAnchorPos; // Here just to be here
    public Vector3 playerAnchorPos; // Set far away from grid
    public FormationData playerFormation; // Set to formation created for grid
    public ShipHolder shipHolder; // See all player ship instances

    IEnumerator Start()
    {
        gridPosition = RunManager.Instance.CurrentRun.currentGridPosition;
        transform.position = gridManager.GetWorldPosition(gridPosition.x, gridPosition.y);

        unitSpawner.SetAnchorPositions(playerAnchorPos, fakeEnemyAnchorPos);
        unitSpawner.SpawnPlayerTeam(RunManager.Instance.CurrentRun.team, playerFormation);

        // Wait until grid exists
        yield return new WaitUntil(() => gridManager.IsGridReady);

        transform.position = gridManager.GetWorldPosition(gridPosition.x, gridPosition.y);

        // Trigger tile on spawn
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
            gridPosition = targetPos;
            transform.position = gridManager.GetWorldPosition(gridPosition.x, gridPosition.y);
            RunManager.Instance.CurrentRun.currentGridPosition = gridPosition;
            SaveManager.Instance.SaveRun();
            HandleTileEvent(tile);
        }
    }

    void HandleTileEvent(TileData tile)
    {
        switch (tile.tileType)
        {
            case TileType.Event:
                HandleRandomEventTile();
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

    public void HandleRandomEventTile()
    {
        Debug.Log("Random Event Triggered!");
    }

    public void HandleEmptyTile()
    {
        Debug.Log("Nothing");
    }

    public void HandleCombatTile(TileData tile)
    {
        RunManager.Instance.CurrentRun.currentGridPosition = gridPosition;
        RunManager.Instance.CurrentRun.currentEncounter = tile.assignedEncounter;
        Debug.Log("Fight: " + tile.assignedEncounter?.encounterName);
        shipHolder.RemovePlayersPassiveEffects();
        SceneManager.LoadScene("SpawnTestScene");
    }

    public void HandlePortalTile()
    {
        Debug.Log("End of run");
        MetaManager.Instance.totalWins++;
        RewardManager.Instance.AddMetaCurrency(100);
        SaveManager.Instance.SaveMeta();
        SaveManager.Instance.DeleteRun();
        SceneManager.LoadScene("TestMainMenu");
    }

    public void HandleShopTile()
    {
        Debug.Log("Entered shop!");
    }
}
