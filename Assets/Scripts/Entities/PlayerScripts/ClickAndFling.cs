using UnityEngine;
using System;

public class ClickAndFling : MonoBehaviour
{
    Camera cam;

    Quaternion startRotation;

    private UnitBase owner;

    Vector3 mouseStart;

    [SerializeField]
    bool flingable = false;

    [SerializeField]
    private GolfBarRotation golfBar;

    [SerializeField]
    ActionType currentActionType;

    public Projectile projectile;

    bool isDragging = false;

    [Header("Force Settings")]
    public float maxDragDistance = 200f;

    public float minMovementForce = 2f;
    public float maxMovementForce = 20f;

    public float minShootingForce = 2f;
    public float maxShootingForce = 20f;

    [SerializeField]
    private UnitActionExecutor executor;

    void Start()
    {
        cam = Camera.main;

        startRotation = transform.rotation;

        owner = GetComponent<UnitBase>();

        if (golfBar == null)
        {
            golfBar = FindFirstObjectByType<GolfBarRotation>();
        }

        if (golfBar == null)
        {
            Debug.Log(
                "ClickAndFling: No GolfBarRotation found in scene, not combat."
            );
        }
    }


    public void SetFlingable(bool value)
    {
        flingable = value;

        if (!value)
        {
            isDragging = false;
            golfBar?.ResetBar();
            FlingEvent.OnPowerChanged?.Invoke(0f);
        }
    }


    public void SetActionType(ActionType type)
    {
        currentActionType = type;
    }

    public void SetProjectile(Projectile newProjectile)
    {
        projectile = newProjectile;
    }

    void Update()
    {
        if (BattleUIManager.Instance != null && BattleUIManager.Instance.IsOverlayOpen())
        {
            isDragging = false;
            return;
        }

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
        if (!flingable)
            return;

        Ray ray =
            cam.ScreenPointToRay(
                Input.mousePosition
            );

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                isDragging = true;

                mouseStart =
                    Input.mousePosition;

                transform.rotation =
                    startRotation;

                FlingEvent.OnPowerChanged?.Invoke(0f);

                golfBar?.BeginDrag(mouseStart);
            }
        }
    }



    void HandleDrag()
    {
        if (!flingable)
            return;

        Vector3 drag =
            Input.mousePosition -
            mouseStart;

        if (drag.magnitude < 1f)
            return;

        float angleY =
            Mathf.Atan2(
                drag.x,
                drag.y
            ) * Mathf.Rad2Deg;

        transform.rotation =
            startRotation *
            Quaternion.Euler(
                0f,
                angleY,
                0f
            );

        float t =
            Mathf.Clamp01(
                drag.magnitude /
                maxDragDistance
            );

        FlingEvent.OnPowerChanged?.Invoke(t);

        golfBar?.UpdateDrag(Input.mousePosition);
    }


    void HandleRelease()
    {
        if (!flingable)
            return;

        Vector3 drag =
            Input.mousePosition -
            mouseStart;

        float dragLength =
            drag.magnitude;

        if (dragLength < 10f)
        {
            CancelPendingAction();
            return;
        }

        float t =
            Mathf.Clamp01(
                dragLength /
                maxDragDistance
            );

        Vector3 direction =
            new Vector3(
                -drag.x,
                0,
                -drag.y
            ).normalized;

        UnitAction action =
            new UnitAction
            {
                actor = owner,
                actionType = currentActionType,
                direction = direction,
                powerPercent = t,
                projectile = projectile
            };

        if (executor == null)
        {
            executor =
                FindFirstObjectByType<UnitActionExecutor>();
        }

        if (executor == null)
        {
            Debug.LogError(
                "No UnitActionExecutor found in scene!"
            );

            CancelPendingAction();
            return;
        }

        // No longer waiting for player targeting.
        flingable = false;

        golfBar?.EndDrag();

        executor.Execute(action);

        FlingEvent.OnPowerChanged?.Invoke(0f);
    }


    public void SetForces(float movement, float shooting)
    {
        maxMovementForce = movement;
        maxShootingForce = shooting;
    }

    public bool IsAwaitingInput()
    {
        return flingable;
    }

    public void CancelPendingAction()
    {
        flingable = false;
        isDragging = false;

        golfBar?.ResetBar();

        FlingEvent.OnPowerChanged?.Invoke(0f);
    }

}