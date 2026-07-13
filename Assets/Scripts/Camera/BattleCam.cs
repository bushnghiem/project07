using UnityEngine;

public class BattleCam : MonoBehaviour
{
    [Header("Target")]
    public ICameraTarget target;

    private Unit currentUnit;

    [Header("Follow")]
    public Vector3 offset;
    public float smoothTime = 0.2f;

    private Vector3 velocity;

    [Header("Manual Camera Movement")]
    public bool useEdgeScroll = true;
    public bool useKeyboardPan = true;

    public float edgeSize = 25f;
    public float moveSpeed = 15f;
    public float maxPanDistance = 8f;

    private Vector3 manualOffset;

    [Header("Locking")]
    public bool controlsEnabled = true;
    public float forceCenterSpeed = 10f;

    private void OnEnable()
    {
        TurnEvent.OnUnitTurnStart += HandleUnitTurnStart;
        DeathEvent.OnEntityDeath += HandleDeath;
        CameraEvent.FollowTarget += FollowTarget;

        CameraEvent.LockCamera += LockCameraToTarget;
        CameraEvent.UnlockCamera += UnlockCameraControls;
        CameraEvent.RecenterCamera += RecenterImmediately;

        TurnEvent.OnUnitActionResolved += HandleUnitActionResolved;

        CameraEvent.AttackFinished += HandleAttackFinished;

    }

    private void OnDisable()
    {
        TurnEvent.OnUnitTurnStart -= HandleUnitTurnStart;
        DeathEvent.OnEntityDeath -= HandleDeath;
        CameraEvent.FollowTarget -= FollowTarget;

        CameraEvent.LockCamera -= LockCameraToTarget;
        CameraEvent.UnlockCamera -= UnlockCameraControls;
        CameraEvent.RecenterCamera -= RecenterImmediately;

        TurnEvent.OnUnitActionResolved -= HandleUnitActionResolved;

        CameraEvent.AttackFinished -= HandleAttackFinished;
    }

    void HandleUnitTurnStart(Unit unit)
    {
        currentUnit = unit;

        if (!(target is Projectile))
        {
            target = unit;
        }

        UnlockCameraControls();
    }

    private void FollowTarget(ICameraTarget newTarget)
    {
        target = newTarget;

        LockCameraToTarget();
    }

    void HandleDeath(Entity entity)
    {
        if (entity == target)
        {
            target = currentUnit;

            UnlockCameraControls();
        }
    }

    void Update()
    {
        HandleManualMovement();
    }

    void HandleManualMovement()
    {
        // Camera is locked to action
        if (!controlsEnabled)
        {
            manualOffset = Vector3.Lerp(manualOffset, Vector3.zero, forceCenterSpeed * Time.deltaTime);
            return;
        }

        Vector3 input = Vector3.zero;

        // WASD / Arrow Keys
        if (useKeyboardPan)
        {
            input.x += Input.GetAxisRaw("Horizontal");
            input.z += Input.GetAxisRaw("Vertical");
        }

        // Edge Scrolling
        if (useEdgeScroll)
        {
            Vector3 mouse = Input.mousePosition;

            if (mouse.x < edgeSize)
                input.x -= 1;

            if (mouse.x > Screen.width - edgeSize)
                input.x += 1;

            if (mouse.y < edgeSize)
                input.z -= 1;

            if (mouse.y > Screen.height - edgeSize)
                input.z += 1;
        }

        input = input.normalized;

        manualOffset += input * moveSpeed * Time.deltaTime;

        manualOffset = Vector3.ClampMagnitude(manualOffset, maxPanDistance);

        // Optional quick recenter
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RecenterImmediately();
        }
    }

    public void RecenterImmediately()
    {
        manualOffset = Vector3.zero;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = target.Position + offset + manualOffset;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }

    public void LockCameraToTarget()
    {
        controlsEnabled = false;
        manualOffset = Vector3.zero;
    }

    public void UnlockCameraControls()
    {
        controlsEnabled = true;
    }

    public void HandleUnitActionResolved(Unit unit)
    {
        if (unit is Player)
        {
            UnlockCameraControls();
        }
    }

    private void HandleAttackFinished()
    {
        target = currentUnit;

        UnlockCameraControls();

        RecenterImmediately();
    }
}