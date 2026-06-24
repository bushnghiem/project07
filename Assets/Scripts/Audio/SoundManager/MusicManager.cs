using UnityEngine;
using System;

public enum MusicType
{
    TITLE,
    OVERWORLD,
    FIGHT,
    BOSS,
    VICTORY
}

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [SerializeField] private MusicTrack[] tracks;

    private static MusicManager Instance;
    private AudioSource audioSource;

    private MusicType? currentTrack;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    public static void PlayMusic(MusicType music)
    {
        if (Instance == null)
            return;

        if (Instance.currentTrack == music)
            return;

        AudioClip clip = Instance.tracks[(int)music].Clip;

        if (clip == null)
        {
            Debug.LogWarning($"No music assigned for {music}");
            return;
        }

        Instance.audioSource.clip = clip;
        Instance.audioSource.Play();

        Instance.currentTrack = music;
    }

    public static void StopMusic()
    {
        if (Instance == null)
            return;

        Instance.audioSource.Stop();
        Instance.currentTrack = null;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        string[] names = Enum.GetNames(typeof(MusicType));

        Array.Resize(ref tracks, names.Length);

        for (int i = 0; i < tracks.Length; i++)
        {
            tracks[i].name = names[i];
        }
    }
#endif
}

[Serializable]
public struct MusicTrack
{
    [HideInInspector]
    public string name;

    [SerializeField]
    private AudioClip clip;

    public AudioClip Clip => clip;
}
