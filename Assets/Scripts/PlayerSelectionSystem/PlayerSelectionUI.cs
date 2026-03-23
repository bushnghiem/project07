using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionUI : MonoBehaviour
{
    public static PlayerSelectionUI Instance;

    public Transform buttonParent;
    public PlayerButton buttonPrefab;

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

        onPlayerSelected = callback;

        foreach (Transform child in buttonParent)
            Destroy(child.gameObject);

        foreach (var player in players)
        {
            var btn = Instantiate(buttonPrefab, buttonParent);
            btn.Setup(player, OnPlayerClicked);
        }
    }

    private void OnPlayerClicked(Player player)
    {
        onPlayerSelected?.Invoke(player);
        gameObject.SetActive(false);
    }
}