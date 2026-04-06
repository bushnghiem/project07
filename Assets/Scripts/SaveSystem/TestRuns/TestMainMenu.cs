using UnityEngine;
using UnityEngine.SceneManagement;


public class TestMainMenu : MonoBehaviour
{
    [SerializeField] public RunData startingRunData;

    public void StartNewRun()
    {
        if (SaveManager.Instance.LoadRun())
        {
            //Debug.Log("Save Found");
            //SceneManager.LoadScene("TestGrid");
            
            RunManager.Instance.CurrentRun = startingRunData;
            MetaManager.Instance.totalRuns++;
            SaveManager.Instance.SaveMeta();
            SceneManager.LoadScene("TestGrid");
            
        }
        else
        {
            RunManager.Instance.CurrentRun = startingRunData;
            MetaManager.Instance.totalRuns++;
            SaveManager.Instance.SaveMeta();
            SceneManager.LoadScene("TestGrid");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
