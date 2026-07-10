using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionUI : MonoBehaviour
{
    public static PlayerSelectionUI Instance;

    public Transform buttonParent;
    public PlayerButton buttonPrefab;

    public bool Active = false;

    private Action<Player> onPlayerSelected;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Open(List<Player> players, Action<Player> callback)
    {
        gameObject.SetActive(true);
        Active = true;

        TooltipUI.Instance.Hide();

        onPlayerSelected = callback;

        foreach (Transform child in buttonParent)
            Destroy(child.gameObject);

        foreach (Player player in players)
        {
            PlayerButton btn = Instantiate(buttonPrefab, buttonParent);
            btn.Setup(player, OnPlayerClicked);
        }
    }

    private void OnPlayerClicked(Player player)
    {
        onPlayerSelected?.Invoke(player);

        GridUIManager.Instance.ClosePlayerSelection();
        Active = false;
    }
}