using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio/Sound Definition")]
public class SoundDefinition : ScriptableObject
{
    [Header("Clips")]
    public AudioClip[] clips;

    [Header("Volume")]
    [Range(0, 1)]
    public float volume = 1f;

    public Vector2 randomVolume = Vector2.zero;

    [Header("Pitch")]
    public float pitch = 1f;

    public Vector2 randomPitch = new(-0.05f, 0.05f);

    [Header("Playback")]

    public bool spatial = true;

    public float minDistance = 8f;

    public float maxDistance = 120f;

    public AudioRolloffMode rolloff = AudioRolloffMode.Linear;

    public bool loop = false;

    public SoundCategory category;

    [Header("Limiter")]

    public float cooldown = 0f;

    public int maxInstances = 8;

    public int priority = 0;

    [Header("Routing")]
    public AudioMixerGroup mixerGroup;
}