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
        MetaManager.Instance.totalRuns++;
        RunManager.Instance.CurrentRun.rng = new RunRNG(RunManager.Instance.CurrentRun.runSeed);
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
