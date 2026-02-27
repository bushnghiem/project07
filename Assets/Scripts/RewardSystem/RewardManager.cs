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

    public void AddMetaCurrency(int additionAmount)
    {
        MetaManager.Instance.metaCurrency += additionAmount;
    }
}
