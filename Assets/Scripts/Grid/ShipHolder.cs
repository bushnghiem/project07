using System.Collections.Generic;
using UnityEngine;

public class ShipHolder : MonoBehaviour
{
    public List<Player> allPlayers = new List<Player>();

    private void OnEnable()
    {
        DeathEvent.OnEntityDeath += HandleDeath;
        SpawnEvent.OnUnitSpawned += HandleUnitSpawned;
    }

    private void OnDisable()
    {
        DeathEvent.OnEntityDeath -= HandleDeath;
        SpawnEvent.OnUnitSpawned -= HandleUnitSpawned;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RewardManager.Instance.shipHolder = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemovePlayersPassiveEffects()
    {
        foreach (var player in allPlayers)
        {
            player.RemoveAllPassiveEffects();
        }

    }

    public void HandleUnitSpawned(Unit unit)
    {
        if (unit is Player player)
        {
            allPlayers.Add(player);
        }
    }

    private void HandleDeath(Entity deadEntity)
    {
        if (deadEntity is Player player)
        {
            allPlayers.Remove(player);
        }
    }
}
