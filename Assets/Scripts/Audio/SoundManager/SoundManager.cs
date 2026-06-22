using UnityEngine;
using System;

public enum SoundType
{
    BUTTON,
    VICTORY,
    DEFEAT,
    ERROR
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;

    private static SoundManager Instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("SoundManager already exists, destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1f)
    {
        if (Instance == null)
        {
            Debug.LogWarning("No SoundManager instance found.");
            return;
        }

        AudioClip[] clips = Instance.soundList[(int)sound].Sounds;

        if (clips == null || clips.Length == 0)
        {
            Debug.LogWarning($"No clips assigned for sound type {sound}.");
            return;
        }

        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        Instance.audioSource.PlayOneShot(randomClip, volume);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        string[] names = Enum.GetNames(typeof(SoundType));

        Array.Resize(ref soundList, names.Length);

        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }
#endif
}

[Serializable]
public struct SoundList
{
    [HideInInspector]
    public string name;

    [SerializeField]
    private AudioClip[] sounds;

    public AudioClip[] Sounds => sounds;
}