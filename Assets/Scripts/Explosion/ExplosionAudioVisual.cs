using UnityEngine;

public class ExplosionAudioVisual : MonoBehaviour
{
    void Start()
    {
        GetComponent<ParticleSystem>().Play();

        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
            audio.Play();

        Destroy(gameObject, 2f);
    }
}
