using UnityEngine;

public class GolfBarRotation : MonoBehaviour
{
    public RectTransform barRoot;

    private Vector2 dragStartPos;
    private bool dragging;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPos = Input.mousePosition;
            dragging = true;
            barRoot.gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
            barRoot.gameObject.SetActive(false);
        }

        if (dragging)
        {
            RotateBar();
        }
    }

    void RotateBar()
    {
        Vector2 currentMousePos = Input.mousePosition;
        Vector2 dragVector = dragStartPos - currentMousePos;

        if (dragVector.sqrMagnitude < 0.01f)
            return;

        float angle = Mathf.Atan2(dragVector.y, dragVector.x) * Mathf.Rad2Deg;
        barRoot.rotation = Quaternion.Euler(0, 0, angle);
    }
}

