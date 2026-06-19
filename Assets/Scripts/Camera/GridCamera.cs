using UnityEngine;
using System.Collections;

public class GridCamera : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private float zOffset = -10f;

    private IEnumerator Start()
    {
        // Wait for grid generation to finish
        yield return new WaitUntil(() => gridManager.IsGridReady);

        CenterOnGrid();
    }

    private void CenterOnGrid()
    {
        Vector3 center = gridManager.GetGridCenterWorldPosition();

        transform.position = new Vector3(
            center.x,
            center.y,
            zOffset
        );
    }
}