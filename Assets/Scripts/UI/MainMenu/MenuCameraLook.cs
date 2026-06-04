using UnityEngine;

public class MenuCameraLook : MonoBehaviour
{
    [SerializeField] private float maxRotation = 3f;
    [SerializeField] private float smoothSpeed = 3f;

    private Quaternion startRotation;

    private void Start()
    {
        startRotation = transform.rotation;
    }

    private void Update()
    {
        float mouseX = (Input.mousePosition.x / Screen.width - 0.5f) * 2f;
        float mouseY = (Input.mousePosition.y / Screen.height - 0.5f) * 2f;

        Quaternion targetRotation =
            startRotation *
            Quaternion.Euler(
                -mouseY * maxRotation,
                mouseX * maxRotation,
                0f);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * smoothSpeed);
    }
}