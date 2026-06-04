using UnityEngine;

public class MenuCameraParallax : MonoBehaviour
{
    [SerializeField] private float maxOffset = 0.4f;
    [SerializeField] private float smoothSpeed = 3f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        // Mouse position normalized from -1 to 1
        float mouseX = (Input.mousePosition.x / Screen.width - 0.5f) * 2f;
        float mouseY = (Input.mousePosition.y / Screen.height - 0.5f) * 2f;

        Vector3 targetPosition = startPosition + new Vector3(
            mouseX * maxOffset,
            mouseY * maxOffset,
            0f
        );

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPosition,
            Time.deltaTime * smoothSpeed
        );
    }
}