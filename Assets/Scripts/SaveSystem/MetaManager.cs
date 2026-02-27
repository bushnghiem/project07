using UnityEngine;
using System.Collections.Generic;

public class MetaManager : MonoBehaviour
{
    public static MetaManager Instance;

    public List<ShipMetaData> metaShips = new();
    public int metaCurrency;
    public int totalWins;
    public int totalRuns;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("MetaManager exists, delete");
            Destroy(gameObject);
        }
    }

    public void InitializeFreshMeta()
    {
        metaShips = new List<ShipMetaData>();
        metaCurrency = 0;
        totalWins = 0;
        totalRuns = 0;
    }
}
