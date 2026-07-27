using UnityEngine;

public class AudioHandle
{
    private PooledAudioSource source;
    private SoundDefinition sound;

    public bool IsPlaying => source != null && source.IsBusy;

    public AudioHandle(
        PooledAudioSource source,
        SoundDefinition sound)
    {
        this.source = source;
        this.sound = sound;
    }

    public void Stop()
    {
        if (source == null)
            return;

        source.Stop();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ReleaseInstance(sound);
        }

        source = null;
    }

    public void SetPosition(Vector3 position)
    {
        if (source == null)
            return;

        source.transform.position = position;
    }
}