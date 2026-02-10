using UnityEngine;

public class ClickAndFling : MonoBehaviour
{
    Camera cam;
    Rigidbody rb;
    Quaternion startRotation;

    Vector3 mouseStart; float zDistance;
    [SerializeField] bool flingable = true;

    [Header("Force Settings")]
    public float minForce = 2f;
    public float maxForce = 20f;
    public float maxDragDistance = 200f;
    public float linearDamping = 2.0f; public float angularDamping = 2.0f;

    void Start()
    { cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = linearDamping;
        rb.angularDamping = angularDamping;
        startRotation = transform.rotation;
    }

    void OnMouseDown()
    {
        if (!flingable) return;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        mouseStart = Input.mousePosition;
        transform.rotation = startRotation;
        FlingEvent.OnPowerChanged?.Invoke(0f);
    }

    void OnMouseDrag()
    {
        if (!flingable) return;
        Vector3 drag = Input.mousePosition - mouseStart;
        if (drag.magnitude < 1f) return;
        float angleY = Mathf.Atan2(drag.x, drag.y) * Mathf.Rad2Deg;
        transform.rotation = startRotation * Quaternion.Euler(0f, angleY, 0f);
        float t = Mathf.Clamp01(drag.magnitude / maxDragDistance);
        FlingEvent.OnPowerChanged?.Invoke(t);
    
    }

    void OnMouseUp()
    {
        if (!flingable) return;
        Vector3 mouseEnd = Input.mousePosition;
        Vector3 drag = mouseEnd - mouseStart;
        float dragLength = drag.magnitude;
        if (dragLength < 10f) return;
        float t = Mathf.Clamp01(dragLength / maxDragDistance);
        float forceStrength = Mathf.Lerp(minForce, maxForce, t);
        Vector3 direction = new Vector3(-drag.x, 0, -drag.y);
        direction.Normalize();
        rb.AddForce(direction * forceStrength, ForceMode.Impulse);
        //Debug.Log(forceStrength);
        FlingEvent.OnFling?.Invoke(direction, forceStrength);
        FlingEvent.OnPowerChanged?.Invoke(0f);
    }
}
