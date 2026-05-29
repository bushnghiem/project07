using UnityEngine;
using UnityEngine.UI;

public class ItemDebugUI : MonoBehaviour
{
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text stateText;
    [SerializeField] private Text cooldownText;

    private UnitBase currentUnit;

    public void SetUnit(UnitBase unit)
    {
        currentUnit = unit;
    }

    private void Update()
    {
        if (currentUnit == null)
            return;

        ActiveItemInstance item = currentUnit.GetActiveItem();

        if (item == null)
        {
            itemNameText.text = "No Item";
            stateText.text = "";
            cooldownText.text = "";
            return;
        }

        itemNameText.text = item.itemData.itemName;

        if (!item.CanUse())
        {
            stateText.text = "On Cooldown";
            cooldownText.text = "CD: " + item.GetRemainingCooldown();
        }
        else
        {
            stateText.text = "Ready";
            cooldownText.text = "";
        }
    }
}