using UnityEngine;
using System.IO;
using System.Collections.Generic;

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
            version = 1,
            ships = MetaManager.Instance.metaShips,
            metaCurrency = MetaManager.Instance.metaCurrency,
            totalWins = MetaManager.Instance.totalWins,
            totalRuns = MetaManager.Instance.totalRuns
        };

        string json = JsonUtility.ToJson(meta, true);
        File.WriteAllText(metaPath, json);

        Debug.Log("Meta Saved (v" + meta.version + ")");
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

        if (meta == null)
        {
            Debug.LogWarning("Meta file corrupted - resetting");
            MetaManager.Instance.InitializeFreshMeta();
            SaveMeta();
            return;
        }

        if (meta.version < 1)
        {
            Debug.LogWarning("Old meta version detected."); // If needed, will add migration logic
        }

        MetaManager.Instance.metaShips = meta.ships ?? new List<ShipMetaData>();
        MetaManager.Instance.metaCurrency = meta.metaCurrency;
        MetaManager.Instance.totalWins = meta.totalWins;
        MetaManager.Instance.totalRuns = meta.totalRuns;

        Debug.Log("Meta Loaded (v" + meta.version + ")");
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

    public bool LoadRun()
    {
        if (!File.Exists(runPath))
        {
            Debug.Log("No Run Save Found");
            return false;
        }

        string json = File.ReadAllText(runPath);
        RunSaveFile run = JsonUtility.FromJson<RunSaveFile>(json);

        RunManager.Instance.CurrentRun = run.currentRun;

        Debug.Log("Run Loaded");
        return true;
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
