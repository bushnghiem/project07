using UnityEngine;

[CreateAssetMenu(menuName = "Ships/Audio Data")]
public class ShipAudioData : ScriptableObject
{
    [Header("Actions")]
    public AudioClip[] moveSounds;
    public AudioClip[] shootSounds;
    public AudioClip[] collisionSounds;
}