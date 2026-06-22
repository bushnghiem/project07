using UnityEngine;

public class ShipAudioComponent : MonoBehaviour
{
    private UnitBase unit;
    private AudioController audioController;

    private ShipAudioData audioData;

    private void Awake()
    {
        unit = GetComponent<UnitBase>();
        audioController = GetComponent<AudioController>();
    }

    private void Start()
    {
        audioData = unit.Template.AudioData;
    }

    public void PlayMove()
    {
        audioController.PlayRandom(
            audioData.moveSounds
        );
    }

    public void PlayShoot()
    {
        audioController.PlayRandom(
            audioData.shootSounds
        );
    }

    public void PlayCollision()
    {
        audioController.PlayRandom(
            audioData.collisionSounds
        );
    }
}
