using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour
{
    public int minimum = 0;
    public int maximum = 100;

    [Header("UI")]
    public Image mask;
    public Image fill;
    public Color color = Color.green;

    private int current;

    void OnEnable()
    {
        FlingEvent.OnPowerChanged += SetPower;
    }

    void OnDisable()
    {
        FlingEvent.OnPowerChanged -= SetPower;
    }

    void SetPower(float t)
    {
        current = Mathf.RoundToInt(Mathf.Lerp(minimum, maximum, t));
        UpdateFill();
    }

#if UNITY_EDITOR
    void Update()
    {
        // Allows previewing in editor when values change
        UpdateFill();
    }
#endif

    void UpdateFill()
    {
        float currentOffset = current - minimum;
        float maximumOffset = maximum - minimum;

        if (maximumOffset <= 0f) return;

        float fillAmount = currentOffset / maximumOffset;
        mask.fillAmount = fillAmount;
        fill.color = color;
    }
}
