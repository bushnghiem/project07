using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerButton : MonoBehaviour
{
    public Button button;
    public TMP_Text nameText;
    public TMP_Text hpText;

    private Player player;
    private Action<Player> callback;

    public void Setup(Player player, Action<Player> callback)
    {
        this.player = player;
        this.callback = callback;

        nameText.text = player.name;
        hpText.text = $"HP: {player.GetCurrentHealth()}";

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => callback(player));
    }
}