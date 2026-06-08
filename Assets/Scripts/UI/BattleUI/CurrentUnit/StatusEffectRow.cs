using TMPro;
using UnityEngine;

public class StatusEffectRowUI : MonoBehaviour
{
    [SerializeField] private TMP_Text effectNameText;
    [SerializeField] private TMP_Text stacksText;
    [SerializeField] private TMP_Text durationText;

    public void Setup(StatusEffectInstance effect)
    {
        effectNameText.text = effect.data.displayName;

        stacksText.text =
            effect.data.isStackable
            ? $"x{effect.Stacks}"
            : "";

        durationText.text =
            $"{effect.RemainingDuration}T";
    }
}