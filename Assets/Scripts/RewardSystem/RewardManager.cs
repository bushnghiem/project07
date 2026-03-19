using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;
    public ShipHolder shipHolder;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("RewardManager exists, delete");
            Destroy(gameObject);
        }
    }

    public void AddRunCurrency(int additionAmount)
    {
        RunManager.Instance.CurrentRun.runCurrency += additionAmount;
    }

    public bool SpendRunCurrency(int amount)
    {
        if (CanAfford(amount))
        {
            RunManager.Instance.CurrentRun.runCurrency -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void HealAllPlayers(int value)
    {
        foreach (var player in shipHolder.allPlayers)
        {
            player.Heal((float)value);
        }
    }

    public void HealPlayer(Player player, int value)
    {
        player.Heal((float)value);
    }

    public void DamageAllPlayers(int value)
    {
        foreach (var player in shipHolder.allPlayers)
        {
            player.Hurt((float)value);
        }
    }

    public void HurtPlayer(Player player, int value)
    {
        player.Hurt((float)value);
    }

    public void AddItemToAllPlayers(Item item)
    {
        foreach (var player in shipHolder.allPlayers)
        {
            item.OnAcquire(player);
            player.AddItemToRunData(item);
        }
    }

    public void AddItemToPlayer(Player player, Item item)
    {
        item.OnAcquire(player);
        player.AddItemToRunData(item);
    }

    public bool CanAfford(int amount)
    {
        return RunManager.Instance.CurrentRun.runCurrency >= amount;
    }

    public void AddMetaCurrency(int additionAmount)
    {
        MetaManager.Instance.metaCurrency += additionAmount;
    }
}
