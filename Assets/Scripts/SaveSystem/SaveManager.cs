using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string metaPath;
    private string runPath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            metaPath = Path.Combine(Application.persistentDataPath, "meta_save.json");
            runPath = Path.Combine(Application.persistentDataPath, "run_save.json");
        }
        else
        {
            Debug.Log("SaveManager exists, delete");
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        TurnEvent.OnFightWon += HandleFightWin;
    }

    private void OnDisable()
    {
        TurnEvent.OnFightWon -= HandleFightWin;
    }

    public void SaveMeta()
    {
        MetaSaveFile meta = new MetaSaveFile
        {
            ships = MetaManager.Instance.metaShips,
            playerCurrency = MetaManager.Instance.playerCurrency,
            totalWins = MetaManager.Instance.totalWins,
            totalRuns = MetaManager.Instance.totalRuns
        };

        string json = JsonUtility.ToJson(meta, true);
        File.WriteAllText(metaPath, json);

        Debug.Log("Meta Saved");
    }

    public void LoadMeta()
    {
        if (!File.Exists(metaPath))
        {
            Debug.Log("No Meta Save Found - Creating New");
            MetaManager.Instance.InitializeFreshMeta();
            SaveMeta();
            return;
        }

        string json = File.ReadAllText(metaPath);
        MetaSaveFile meta = JsonUtility.FromJson<MetaSaveFile>(json);

        MetaManager.Instance.metaShips = meta.ships ?? new();
        MetaManager.Instance.playerCurrency = meta.playerCurrency;
        MetaManager.Instance.totalWins = meta.totalWins;
        MetaManager.Instance.totalRuns = meta.totalRuns;

        Debug.Log("Meta Loaded");
    }

    public void SaveRun()
    {
        RunSaveFile run = new RunSaveFile
        {
            currentRun = RunManager.Instance.CurrentRun
        };

        string json = JsonUtility.ToJson(run, true);
        File.WriteAllText(runPath, json);

        Debug.Log("Run Saved");
    }

    public void LoadRun()
    {
        if (!File.Exists(runPath))
        {
            Debug.Log("No Run Save Found");
            return;
        }

        string json = File.ReadAllText(runPath);
        RunSaveFile run = JsonUtility.FromJson<RunSaveFile>(json);

        RunManager.Instance.CurrentRun = run.currentRun;

        Debug.Log("Run Loaded");
    }

    public void DeleteRun()
    {
        if (File.Exists(runPath))
        {
            File.Delete(runPath);
            Debug.Log("Run Save Deleted");
        }

        RunManager.Instance.CurrentRun = null;
    }

    private void HandleFightWin()
    {
        SaveRun();
    }

    private void HandleFightLost()
    {
        DeleteRun();
        SaveMeta();
    }
}
