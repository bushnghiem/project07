using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioListener listener;

    [SerializeField]
    private AudioPool pool;

    Dictionary<SoundDefinition, float> cooldowns =
        new();

    Dictionary<SoundDefinition, int> instances =
        new();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        listener = FindFirstObjectByType<AudioListener>();
    }

    public Vector3 ListenerPosition
    {
        get
        {
            if (listener == null)
                listener = FindFirstObjectByType<AudioListener>();

            return listener != null
                ? listener.transform.position
                : Vector3.zero;
        }
    }

    public static void Play(SoundDefinition sound)
    {
        if (Instance == null || sound == null)
            return;

        Instance.InternalPlay(sound, Vector3.zero);
    }

    public static AudioHandle Play(
        SoundDefinition sound,
        Vector3 position)
    {
        if (Instance == null || sound == null)
            return null;

        return Instance.InternalPlay(sound, position);
    }

    AudioHandle InternalPlay(
        SoundDefinition sound,
        Vector3 position)
    {
        if (sound.spatial)
        {
            position.y = ListenerPosition.y;
        }

        if (sound.clips == null || sound.clips.Length == 0)
            return null;

        // Cooldown

        if (cooldowns.TryGetValue(sound, out float next))
        {
            if (Time.time < next)
                return null;

        }

        cooldowns[sound] = Time.time + sound.cooldown;

        // Max instances

        if (!instances.ContainsKey(sound))
            instances[sound] = 0;

        if (instances[sound] >= sound.maxInstances)
            return null;


        if (pool == null)
        {
            Debug.LogError("AudioPool missing from AudioManager");
            return null;

        }

        PooledAudioSource source =
            pool.GetFreeSource();

        if (source == null)
            return null;


        instances[sound]++;

        source.Play(sound, position);

        if (!sound.loop)
        {
            StartCoroutine(
                RemoveInstance(sound, source));
        }
        return new AudioHandle(source, sound);
    }

    System.Collections.IEnumerator RemoveInstance(
    SoundDefinition sound,
    PooledAudioSource src)
    {
        yield return new WaitWhile(() => src.IsBusy);

        instances[sound] = Mathf.Max(0, instances[sound] - 1);
    }

    public void ReleaseInstance(SoundDefinition sound)
    {
        if (!instances.ContainsKey(sound))
            return;

        instances[sound] = Mathf.Max(
            0,
            instances[sound] - 1);
    }
}