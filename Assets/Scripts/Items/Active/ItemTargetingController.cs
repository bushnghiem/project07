using UnityEngine;
using System;

public class ItemTargetingController : MonoBehaviour
{
    private UnitBase currentUser;

    private ActiveItemInstance currentItem;

    [SerializeField]
    private UnitActionExecutor executor;

    private bool targeting = false;

    public bool IsTargeting => targeting;

    public ActiveItemInstance CurrentItem => currentItem;

    public UnitBase CurrentUser => currentUser;

    public event Action<ActiveItemInstance> OnTargetingStarted;
    public event Action OnTargetingEnded;

    [SerializeField]
    private RangeIndicator rangeIndicatorPrefab;

    private RangeIndicator activeIndicator;

    private GameObject activePreviewObject;
    private Renderer activePreviewRenderer;

    private RaycastHit lastHit;

    private LineRenderer coneRenderer;

    public void BeginTargeting(UnitBase user, ActiveItemInstance item)
    {
        Debug.Log("Item range: " + item.itemData.range);
        if (user == null)
        {
            Debug.LogWarning("BeginTargeting failed: user is null");
            return;
        }

        if (item == null || item.itemData == null)
        {
            Debug.LogWarning("BeginTargeting failed: item is null");
            return;
        }

        if (targeting)
            return;

        currentUser = user;
        currentItem = item;

        if (item.itemData.TargetType == ItemTargetType.Self)
        {
            ConfirmSelf();
            return;
        }

        targeting = true;

        if (item.itemData.TargetType != ItemTargetType.Direction)
        {
            activeIndicator = Instantiate(rangeIndicatorPrefab);

            activeIndicator.Setup(currentUser.Position, currentItem.itemData.range);
        }

        OnTargetingStarted?.Invoke(currentItem);
    }

    private void Update()
    {
        if (BattleUIManager.Instance != null &&
            BattleUIManager.Instance.IsOverlayOpen())
        {
            return;
        }

        if (!targeting)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            lastHit = hit;
            if (currentItem.itemData.TargetType == ItemTargetType.Unit)
            {
                UnitBase unit = GetUnitUnderMouse(hit);

                if (unit != null)
                {
                    UpdateUnitPreview(unit);
                }
            }
            else if (currentItem.itemData.TargetType == ItemTargetType.Direction)
            {
                UpdateDirectionPreview(hit.point);
            }
            else
            {
                UpdatePreview(hit.point);
            }

            if (Input.GetMouseButtonDown(0))
            {
                HandleClick();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            CancelTargeting();
        }
    }

    private void UpdatePreview(Vector3 point)
    {
        if (currentItem == null || currentItem.itemData.previewPrefab == null)
            return;

        if (activePreviewObject == null)
        {
            activePreviewObject = Instantiate(currentItem.itemData.previewPrefab);
            activePreviewRenderer = activePreviewObject.GetComponentInChildren<Renderer>();
        }

        activePreviewObject.transform.position = point;

        bool valid = IsInRange(point);

        if (activePreviewRenderer != null)
        {
            activePreviewRenderer.material.color =
                valid ? currentItem.itemData.validColor : currentItem.itemData.invalidColor;
        }
    }

    private void UpdateUnitPreview(UnitBase unit)
    {
        if (currentItem == null || currentItem.itemData.previewPrefab == null)
            return;

        if (activePreviewObject == null)
        {
            activePreviewObject = Instantiate(currentItem.itemData.previewPrefab);
            activePreviewRenderer = activePreviewObject.GetComponentInChildren<Renderer>();
        }

        activePreviewObject.transform.position = unit.Position;

        float dist = Vector3.Distance(currentUser.Position, unit.Position);
        bool valid = dist <= currentItem.itemData.range;

        if (activePreviewRenderer != null)
        {
            activePreviewRenderer.material.color =
                valid ? currentItem.itemData.validColor : currentItem.itemData.invalidColor;
        }
    }

    private void UpdateDirectionPreview(Vector3 worldPoint)
    {
        if (currentItem == null || currentItem.itemData.previewPrefab == null)
            return;

        if (activePreviewObject == null)
        {
            activePreviewObject = Instantiate(currentItem.itemData.previewPrefab);
            activePreviewRenderer = activePreviewObject.GetComponentInChildren<Renderer>();

            coneRenderer = activePreviewObject.GetComponentInChildren<LineRenderer>();
        }

        float radius = currentItem.itemData.effectRadius;
        float angle = currentItem.itemData.coneAngle;

        Vector3 dir = worldPoint - currentUser.Position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f)
            return;

        activePreviewObject.transform.position = currentUser.Position;
        activePreviewObject.transform.rotation = Quaternion.LookRotation(dir);

        DrawCone(currentUser.Position, dir, angle, radius);

        bool valid = dir.magnitude <= radius;

        if (activePreviewRenderer != null)
        {
            activePreviewRenderer.material.color =
                valid ? currentItem.itemData.validColor : currentItem.itemData.invalidColor;
        }
    }

    private void ClearPreview()
    {
        if (activePreviewObject != null)
        {
            Destroy(activePreviewObject);
            activePreviewObject = null;
            activePreviewRenderer = null;
        }
    }

    private void HandleClick()
    {
        switch (currentItem.itemData.TargetType)
        {
            case ItemTargetType.Self:
                ConfirmSelf();
                break;

            case ItemTargetType.Position:
                ConfirmPosition();
                break;

            case ItemTargetType.Unit:
                ConfirmUnit();
                break;

            case ItemTargetType.Direction:
                Vector3 dir = (lastHit.point - currentUser.Position);
                dir.y = 0f;

                ItemTargetData data = new ItemTargetData
                {
                    direction = dir.normalized
                };

                ExecuteItem(data);
                break;
        }
    }

    private void ConfirmSelf()
    {
        ItemTargetData data =
            new ItemTargetData
            {
                targetUnit = currentUser
            };

        ExecuteItem(data);
    }

    private void ConfirmPosition()
    {
        Ray ray =
            Camera.main.ScreenPointToRay(
                Input.mousePosition
            );

        if (
            Physics.Raycast(
                ray,
                out RaycastHit hit
            )
        )
        {
            if (!IsInRange(hit.point))
            {
                Debug.Log("Out of range");
                return;
            }

            ItemTargetData data =
                new ItemTargetData
                {
                    targetPosition = hit.point
                };

            ExecuteItem(data);
        }
        Debug.Log("Hit object: " + hit.collider.name);
    }

    private void ConfirmUnit()
    {
        Ray ray =
            Camera.main.ScreenPointToRay(
                Input.mousePosition
            );

        if (
            Physics.Raycast(
                ray,
                out RaycastHit hit
            )
        )
        {
            UnitBase unit = hit.collider.GetComponentInParent<UnitBase>();



            if (unit == null)
                return;

            if (!IsInRange(unit.Position))
            {
                Debug.Log("Target out of range");
                return;
            }

            ItemTargetData data =
                new ItemTargetData
                {
                    targetUnit = unit
                };

            ExecuteItem(data);
        }
    }

    private void ExecuteItem(ItemTargetData data)
    {
        if (executor == null)
        {
            executor = FindFirstObjectByType<UnitActionExecutor>();
        }

        if (executor == null)
        {
            Debug.LogError("No UnitActionExecutor found!");
            return;
        }

        if (currentUser == null || currentItem == null)
        {
            Debug.LogWarning("ExecuteItem aborted: missing state");
            return;
        }

        UnitAction action = new UnitAction
        {
            actor = currentUser,
            actionType = ActionType.Item,
            activeItem = currentItem,
            itemTargetData = data
        };

        executor.Execute(action);

        targeting = false;
        OnTargetingEnded?.Invoke();

        if (activeIndicator != null)
        {
            Destroy(activeIndicator.gameObject);
            activeIndicator = null;
        }

        ClearPreview();

        currentUser = null;
        currentItem = null;
    }

    public void CancelTargeting()
    {
        targeting = false;
        OnTargetingEnded?.Invoke();

        if (activeIndicator != null)
        {
            Destroy(activeIndicator.gameObject);
            activeIndicator = null;
        }

        ClearPreview();

        currentUser = null;
        currentItem = null;

        Debug.Log("Cancelled targeting");
    }

    private bool IsInRange(Vector3 pos)
    {
        float dist =
            Vector3.Distance(
                currentUser.Position,
                pos
            );

        return dist <=
            currentItem.itemData.range;
    }

    private UnitBase GetUnitUnderMouse(RaycastHit hit)
    {
        return hit.collider.GetComponentInParent<UnitBase>();
    }

    private void DrawCone(Vector3 origin, Vector3 dir, float angle, float radius)
    {
        if (coneRenderer == null) return;

        int segments = 24;
        float halfAngle = angle * 0.5f;

        coneRenderer.positionCount = segments + 2;

        coneRenderer.SetPosition(0, origin);

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;

            float currentAngle = Mathf.Lerp(-halfAngle, halfAngle, t);
            Quaternion rot = Quaternion.AngleAxis(currentAngle, Vector3.up);

            Vector3 pointDir = rot * dir.normalized;
            Vector3 point = origin + pointDir * radius;

            coneRenderer.SetPosition(i + 1, point);
        }
    }
}