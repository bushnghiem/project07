using UnityEngine;
using UnityEngine.SceneManagement;

public class GridMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector2Int gridPosition;
    public GridManager gridManager;

    void Start()
    {
        gridPosition = RunManager.Instance.CurrentRun.currentGridPosition;
        transform.position = gridManager.GetWorldPosition(gridPosition.x, gridPosition.y);
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
                HandleCombatTile();
                break;
            case TileType.Portal:
                HandlePortalTile();
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

    public void HandleCombatTile()
    {
        RunManager.Instance.CurrentRun.currentGridPosition = gridPosition;
        Debug.Log("Fight");
        SceneManager.LoadScene("SpawnTestScene");
    }

    public void HandlePortalTile()
    {
        Debug.Log("End of run");
        MetaManager.Instance.totalWins++;
        MetaManager.Instance.playerCurrency += 100;
        SaveManager.Instance.SaveMeta();
        SaveManager.Instance.DeleteRun();
        SceneManager.LoadScene("TestMainMenu");
    }
}
