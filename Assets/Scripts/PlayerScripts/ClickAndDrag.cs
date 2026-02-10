using UnityEngine;

public class ClickAndDrag : MonoBehaviour
{
    Vector3 offset;
    Camera cam;
    float screenZ;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        Debug.Log("Clicked");
        screenZ = cam.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + offset;
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = screenZ;
        return cam.ScreenToWorldPoint(mouseScreenPos);
    }
}
