using UnityEngine;

public class ShipAudioComponent : MonoBehaviour
{
    private UnitBase unit;
    private DamageOnCollision collision;
    private ShipAudioData audioData;

    private void Awake()
    {
        collision = GetComponent<DamageOnCollision>();
        collision.OnCollisionOccurred += PlayCollision;
        unit = GetComponent<UnitBase>();
    }

    private void Start()
    {
        audioData = unit.Template.AudioData;
    }

    public void PlayMove()
    {
        AudioManager.Play(audioData.Move, transform.position);
    }

    public void PlayShoot()
    {
        AudioManager.Play(audioData.Shoot, transform.position);
    }

    public void PlayCollision()
    {
        AudioManager.Play(audioData.Collision, transform.position);
    }
}