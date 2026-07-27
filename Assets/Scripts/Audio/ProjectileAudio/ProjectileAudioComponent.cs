using UnityEngine;

public class ProjectileAudioComponent : MonoBehaviour
{
    private DamageOnCollision collision;
    private ProjectileAudioData audioData;

    public void Initialize(ProjectileAudioData data)
    {
        collision = GetComponent<DamageOnCollision>();
        collision.OnCollisionOccurred += PlayCollision;
        audioData = data;
    }

    public void PlayLaunch()
    {
        AudioManager.Play(audioData.Launch, transform.position);
    }

    public void PlayDeath()
    {
        AudioManager.Play(audioData.Death, transform.position);
    }

    public void PlayCollision()
    {
        AudioManager.Play(audioData.Collision, transform.position);
    }
}