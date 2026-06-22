using UnityEngine;

[CreateAssetMenu(menuName = "Projectiles/Audio Data")]
public class ProjectileAudioData : ScriptableObject
{
    [Header("Lifecycle")]
    public AudioClip[] launchSounds;
    public AudioClip[] collisionSounds;
    public AudioClip[] deathSounds;
}