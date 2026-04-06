using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance;

    public RunData CurrentRun;

    [Header("Floor Content Profiles")]
    public FloorContentProfile earlyGameProfile;
    public FloorContentProfile midGameProfile;
    public FloorContentProfile lateGameProfile;

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

    public FloorContentProfile GetProfileForFloor(int floor)
    {
        Debug.Log("Assign floor profile");
        if (floor < 3 && earlyGameProfile != null) return earlyGameProfile;
        if (floor < 6 && midGameProfile != null) return midGameProfile;
        if (lateGameProfile != null) return lateGameProfile;

        Debug.LogError("No FloorContentProfile assigned!");
        return null;
    }
}
