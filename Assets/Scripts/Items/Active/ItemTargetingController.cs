using UnityEngine;

public class ItemTargetingController : MonoBehaviour
{
    private UnitBase currentUser;

    private ActiveItemInstance currentItem;

    [SerializeField]
    private UnitActionExecutor executor;

    private bool targeting = false;

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

        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }

        if (Input.GetMouseButtonDown(1))
        {
            CancelTargeting();
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
        currentUser = null;
        currentItem = null;
    }

    public void CancelTargeting()
    {
        targeting = false;
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

    public bool IsTargeting()
    {
        return targeting;
    }
}