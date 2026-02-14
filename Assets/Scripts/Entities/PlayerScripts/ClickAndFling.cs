using UnityEngine;

public class ClickAndFling : MonoBehaviour
{
    Camera cam;
    Rigidbody rb;
    Quaternion startRotation;

    Vector3 mouseStart; float zDistance;
    [SerializeField] bool flingable = false;
    [SerializeField] bool projectileMode = false;
    [SerializeField] float projectileSpawnRadius = 2.0f;
    bool isDragging = false;

    [Header("Force Settings")]
    public float minForce = 2f;
    public float maxForce = 20f;
    public float maxDragDistance = 200f;
    public float linearDamping = 2.0f;
    public float angularDamping = 2.0f;

    void Start()
    { 
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = linearDamping;
        rb.angularDamping = angularDamping;
        startRotation = transform.rotation;
    }

    public void SetFlingable(bool value)
    {
        flingable = value;
    }

    public void SetProjectileMode(bool value)
    {
        projectileMode = value;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryStartDrag();
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            HandleDrag();
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            HandleRelease();
            isDragging = false;
        }
    }

    void TryStartDrag()
    {
        if (!flingable) return;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                isDragging = true;

                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                mouseStart = Input.mousePosition;
                transform.rotation = startRotation;
                FlingEvent.OnPowerChanged?.Invoke(0f);
            }
        }
    }

    void HandleDrag()
    {
        if (!flingable) return;
        Vector3 drag = Input.mousePosition - mouseStart;
        if (drag.magnitude < 1f) return;
        float angleY = Mathf.Atan2(drag.x, drag.y) * Mathf.Rad2Deg;
        transform.rotation = startRotation * Quaternion.Euler(0f, angleY, 0f);
        float t = Mathf.Clamp01(drag.magnitude / maxDragDistance);
        FlingEvent.OnPowerChanged?.Invoke(t);
    }

    void HandleRelease()
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
        Vector3 projectileSpawnPosition = transform.position + direction.normalized * projectileSpawnRadius;
        if (projectileMode)
        {
            ProjectileSpawnEvent.OnProjectileSpawn?.Invoke(projectileSpawnPosition, direction, forceStrength);
            TurnEvent.OnPlayerTurnEnd?.Invoke((Entity)this.transform.parent);
        }
        else
        {
            rb.AddForce(direction * forceStrength, ForceMode.Impulse);
            FlingEvent.OnFling?.Invoke(direction, forceStrength);
            TurnEvent.OnPlayerTurnEnd?.Invoke((Entity)this.transform.parent);
            //Debug.Log(forceStrength);
        }
        FlingEvent.OnPowerChanged?.Invoke(0f);
    }
}
