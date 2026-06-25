using UnityEngine;
using System;

public class RunRNG
{
    private System.Random rng;

    public RunRNG(int seed)
    {
        rng = new System.Random(seed);
    }

    public float NextFloat()
    {
        return (float)rng.NextDouble();
    }

    public int NextInt(int min, int max)
    {
        return rng.Next(min, max);
    }
}
