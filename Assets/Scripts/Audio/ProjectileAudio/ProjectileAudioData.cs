using UnityEngine;

[CreateAssetMenu(menuName = "Projectiles/Audio Data")]
public class ProjectileAudioData : ScriptableObject
{
    public SoundDefinition Launch;
    public SoundDefinition Collision;
    public SoundDefinition Death;
}