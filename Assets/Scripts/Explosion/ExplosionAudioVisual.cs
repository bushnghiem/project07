using UnityEngine;

public class ExplosionAudioVisual : MonoBehaviour
{
    [Header("Intensity")]
    [SerializeField] private float CameraShakeIntensity = 1.5f;

    void Start()
    {
        GetComponent<ParticleSystem>().Play();

        CameraEvent.ShakeCamera?.Invoke(CameraShakeIntensity);

        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
            audio.Play();

        Destroy(gameObject, 2f);
    }
}
