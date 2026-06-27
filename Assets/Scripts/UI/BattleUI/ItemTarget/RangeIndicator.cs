using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    public void Setup(Vector3 center, float radius)
    {
        transform.position = center;

        transform.localScale =
            new Vector3(radius * 2f, 1f, radius * 2f);
    }
}