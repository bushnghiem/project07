using UnityEngine;
using System.Collections.Generic;

public class AudioPool : MonoBehaviour
{
    [SerializeField]
    private PooledAudioSource prefab;

    [SerializeField]
    private int poolSize = 32;

    private List<PooledAudioSource> pool = new();

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            PooledAudioSource src =
                Instantiate(prefab, transform);

            pool.Add(src);
        }
    }

    public PooledAudioSource GetFreeSource()
    {
        foreach (var src in pool)
        {
            if (!src.IsBusy)
                return src;
        }

        PooledAudioSource newSource =
            Instantiate(prefab, transform);

        newSource.name = $"Pooled Audio Source {pool.Count}";

        pool.Add(newSource);

        return newSource;
    }
}