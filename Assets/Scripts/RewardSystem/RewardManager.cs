using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;

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

    public bool CanAfford(int amount)
    {
        return RunManager.Instance.CurrentRun.runCurrency >= amount;
    }

    public void AddMetaCurrency(int additionAmount)
    {
        MetaManager.Instance.metaCurrency += additionAmount;
    }
}
