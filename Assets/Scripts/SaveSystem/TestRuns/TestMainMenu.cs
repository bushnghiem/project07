using UnityEngine;
using UnityEngine.SceneManagement;


public class TestMainMenu : MonoBehaviour
{
    [SerializeField] public RunData startingRunData;

    public void StartNewRun()
    {
        RunData newRun = new RunData();
        newRun.runSeed = Random.Range(int.MinValue, int.MaxValue);
        ShipRunData ship0 = new ShipRunData();
        ActiveItemSaveData itemData = new ActiveItemSaveData();
        itemData.activeItemID = "1";
        ship0.SetDefaults();
        ship0.currentActiveItem = itemData;
        newRun.team.Add(ship0);
        RunManager.Instance.CurrentRun = startingRunData;
        SceneManager.LoadScene("SpawnTestScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
