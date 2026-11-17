using UnityEngine;

public class DestroyWhenParticlesFinished : MonoBehaviour
{
    private ParticleSystem[] particles;

    private void Awake()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    private void Update()
    {
        foreach (var particle in particles)
        {
            if (particle != null && particle.IsAlive(true))
                return;
        }

        Destroy(gameObject);
    }
}