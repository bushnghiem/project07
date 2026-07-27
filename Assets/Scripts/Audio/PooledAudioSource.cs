using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PooledAudioSource : MonoBehaviour
{
    private AudioSource source;

    public bool IsBusy => source.isPlaying;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(SoundDefinition sound, Vector3 position)
    {
        transform.position = position;

        source.clip = sound.clips[
            Random.Range(0, sound.clips.Length)];

        source.loop = sound.loop;

        source.spatialBlend = sound.spatial ? 1 : 0;

        source.minDistance = sound.minDistance;

        source.maxDistance = sound.maxDistance;

        source.rolloffMode = sound.rolloff;

        source.volume = Mathf.Clamp01(
            sound.volume +
            Random.Range(
                sound.randomVolume.x,
                sound.randomVolume.y));

        source.pitch = Mathf.Clamp(
            sound.pitch +
            Random.Range(
                sound.randomPitch.x,
                sound.randomPitch.y),
            0.1f,
            3f);

        source.dopplerLevel = 0f;
        source.spatialize = false;
        source.ignoreListenerPause = false;

        source.outputAudioMixerGroup = sound.mixerGroup;

        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}
