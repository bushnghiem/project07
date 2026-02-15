using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance;

    public RunData CurrentRun;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
