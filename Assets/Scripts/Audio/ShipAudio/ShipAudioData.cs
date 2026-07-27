using UnityEngine;

[CreateAssetMenu(menuName = "Ships/Audio Data")]
public class ShipAudioData : ScriptableObject
{
    public SoundDefinition Move;
    public SoundDefinition Shoot;
    public SoundDefinition Collision;
}