using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public RunData startingRunData;

    [SerializeField] private Button continueButton;

    void Start()
    {
        continueButton.gameObject.SetActive(SaveExists());
    }

    public void Continue()
    {
        SceneManager.LoadScene("TestGrid");
    }

    public void StartNewRun()
    {
        RunManager.Instance.CurrentRun = startingRunData;

        var run = RunManager.Instance.CurrentRun;

        // Generate a new run seed
        run.runSeed = Random.Range(1, int.MaxValue);

        // Give the starting floor a seed based on this run
        if (run.currentFloorData != null)
        {
            run.currentFloorData.floorSeed =
                run.runSeed + run.currentFloor * 1000;
        }

        Debug.Log($"New Run Seed: {run.runSeed}");
        Debug.Log($"Floor Seed: {run.currentFloorData.floorSeed}");

        MetaManager.Instance.totalRuns++;
        SaveManager.Instance.SaveMeta();

        SceneManager.LoadScene("TestGrid");
    }

    public void Options()
    {
        Debug.Log("Options lol");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public bool SaveExists()
    {
        return SaveManager.Instance.LoadRun();
    }
}
