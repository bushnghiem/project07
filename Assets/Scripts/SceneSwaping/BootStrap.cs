using UnityEngine;
using UnityEngine.SceneManagement;

public class BootStrap : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SaveManager.Instance.LoadMeta();
        SceneManager.LoadScene("TestMainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
