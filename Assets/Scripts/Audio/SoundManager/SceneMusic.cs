using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    [SerializeField] private MusicType music;

    private void Start()
    {
        MusicManager.PlayMusic(music);
    }
}
