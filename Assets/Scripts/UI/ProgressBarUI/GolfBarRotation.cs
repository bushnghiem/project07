using UnityEngine;

public class GolfBarRotation : MonoBehaviour
{
    public RectTransform barRoot;

    private Vector2 dragStartPos;
    private bool dragging;

    private void Awake()
    {
        Hide();
    }

    public void BeginDrag(Vector2 startPosition)
    {
        dragStartPos = startPosition;
        dragging = true;

        barRoot.gameObject.SetActive(true);
    }

    public void UpdateDrag(Vector2 currentMousePosition)
    {
        if (!dragging)
            return;

        Vector2 dragVector =
            dragStartPos - currentMousePosition;

        if (dragVector.sqrMagnitude < 0.01f)
            return;

        float angle =
            Mathf.Atan2(
                dragVector.y,
                dragVector.x
            ) * Mathf.Rad2Deg;

        barRoot.rotation =
            Quaternion.Euler(0f, 0f, angle);
    }

    public void EndDrag()
    {
        dragging = false;
        Hide();
    }

    public void ResetBar()
    {
        dragging = false;

        barRoot.rotation =
            Quaternion.identity;

        Hide();
    }

    private void Hide()
    {
        barRoot.gameObject.SetActive(false);
    }
}
