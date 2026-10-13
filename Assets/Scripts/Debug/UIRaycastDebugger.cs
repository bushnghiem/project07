using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIRaycastDebugger : MonoBehaviour
{
    private readonly List<RaycastResult> results = new();

    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (EventSystem.current == null)
            return;

        PointerEventData pointer =
            new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

        results.Clear();

        EventSystem.current.RaycastAll(pointer, results);

        foreach (RaycastResult result in results)
        {
            Debug.Log(
                $"UI Raycast Hit: {result.gameObject.name}",
                result.gameObject
            );
        }
    }
}