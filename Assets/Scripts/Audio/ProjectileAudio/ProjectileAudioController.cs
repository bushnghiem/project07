using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ProjectileAudioController : MonoBehaviour
{
    private AudioSource audioSource;
    private ProjectileAudioData audioData;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Initialize(ProjectileAudioData data)
    {
        audioData = data;
    }

    private void PlayRandom(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return;

        AudioClip clip =
            clips[Random.Range(0, clips.Length)];

        audioSource.PlayOneShot(clip);
    }

    public void PlayLaunch()
    {
        PlayRandom(audioData?.launchSounds);
    }

    public void PlayDeath()
    {
        PlayRandom(audioData?.deathSounds);
    }
}