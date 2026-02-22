using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Path.Combine(Application.persistentDataPath, "save.json");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        TurnEvent.OnFightWon += SaveGame;
    }

    private void OnDisable()
    {
        TurnEvent.OnFightWon -= SaveGame;
    }

    public void SaveGame()
    {
        SaveFile save = new SaveFile();

        save.currentRun = RunManager.Instance.CurrentRun;
        save.playerCurrency = 100; // example

        string json = JsonUtility.ToJson(save, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("No Save Found");
            return;
        }

        string json = File.ReadAllText(savePath);
        SaveFile save = JsonUtility.FromJson<SaveFile>(json);

        RunManager.Instance.CurrentRun = save.currentRun;

        Debug.Log("Game Loaded");
    }

    public void DeleteRunSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Run save deleted.");
        }

        RunManager.Instance.CurrentRun = null;
    }

    private void HandleFightWin()
    {
        SaveGame();
    }

    private void HandleFightLost()
    {
        DeleteRunSave();
        LoadGameOverScreen();
    }

    private void LoadGameOverScreen()
    {
        Debug.Log("Game Over");
    }
}
