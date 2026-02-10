using UnityEngine;

public class UIFollowMouse : MonoBehaviour
{
    public Transform ball;
    public RectTransform barTransform;
    public Canvas canvas;

    void LateUpdate()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(ball.position);
        barTransform.position = screenPos + new Vector2(0, 80);
    }
}
