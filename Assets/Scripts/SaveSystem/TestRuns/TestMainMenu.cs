using UnityEngine;
using UnityEngine.SceneManagement;


public class TestMainMenu : MonoBehaviour
{
    public void StartNewRun()
    {
        RunData newRun = new RunData();
        newRun.runSeed = Random.Range(int.MinValue, int.MaxValue);
        ShipRunData ship0 = new ShipRunData();
        ActiveItemSaveData itemData = new ActiveItemSaveData();
        itemData.itemID = "1";
        ship0.SetDefaults();
        ship0.currentItem = itemData;
        newRun.team.Add(ship0);
        RunManager.Instance.CurrentRun = newRun;
        SceneManager.LoadScene("SpawnTestScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game"); // Only works in build
    }
}
