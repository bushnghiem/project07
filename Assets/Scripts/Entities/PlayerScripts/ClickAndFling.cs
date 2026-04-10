using UnityEngine;
using System;

public class ClickAndFling : MonoBehaviour
{
    Camera cam;
    Rigidbody rb;
    Quaternion startRotation;
    private UnitBase owner;

    Vector3 mouseStart; float zDistance;
    [SerializeField] bool flingable = false;
    [SerializeField] bool projectileMode = false;
    [SerializeField] float projectileSpawnRadius = 2.0f;
    public Projectile projectile;
    bool isDragging = false;

    [Header("Force Settings")]
    public float minMovementForce = 2f;
    public float maxMovementForce = 20f;
    public float minShootingForce = 2f;
    public float maxShootingForce = 20f;
    public float maxDragDistance = 200f;

    public event Action<Vector3, float> OnFling; // direction, force

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        startRotation = transform.rotation;
        owner = GetComponent<UnitBase>();
    }

    public void SetForces(float movement, float shooting)
    {
        maxMovementForce = movement;
        maxShootingForce = shooting;
    }

    public void SetFlingable(bool value)
    {
        flingable = value;
    }

    public void SetProjectileMode(bool value)
    {
        projectileMode = value;
    }

    public bool GetProjectileMode()
    {
        return projectileMode;
    }

    public void SetProjectile(Projectile newProjectile)
    {
        projectile = newProjectile;
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

        Vector3 direction = new Vector3(-drag.x, 0, -drag.y);
        direction.Normalize();

        ExecuteFling(direction, t);
    }

    public void ExecuteFling(Vector3 direction, float t)
    {
        if (!flingable) return;

        direction.Normalize();

        if (projectileMode)
        {
            float forceStrength = Mathf.Lerp(minShootingForce, maxShootingForce, t);

            bool handled = owner != null &&
                           owner.TriggerShootEffects(direction, forceStrength);

            if (!handled)
            {
                Vector3 projectileSpawnPosition = transform.position + direction * projectileSpawnRadius;

                ProjectileSpawnEvent.OnProjectileSpawn?.Invoke(
                    projectileSpawnPosition,
                    direction,
                    forceStrength,
                    projectile,
                    owner
                );
            }

            OnFling?.Invoke(direction, forceStrength);
        }
        else
        {
            float forceStrength = Mathf.Lerp(minMovementForce, maxMovementForce, t);

            rb.AddForce(direction * forceStrength, ForceMode.Impulse);

            OnFling?.Invoke(direction, forceStrength);
        }

        FlingEvent.OnPowerChanged?.Invoke(0f);
    }
}
