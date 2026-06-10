using TMPro;
using UnityEngine;

public class CombatLogRow : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;

    public void Initialize(CombatLogEntry entry)
    {
        messageText.text = entry.Message;
    }
}