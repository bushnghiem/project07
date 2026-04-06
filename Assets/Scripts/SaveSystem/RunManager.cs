using UnityEngine;
using System.Collections.Generic;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance;

    public RunData CurrentRun;

    [Header("Tiered Floor Content Pools")]
    public List<FloorContentProfile> easyFloors;
    public List<FloorContentProfile> midFloors;
    public List<FloorContentProfile> hardFloors;

    [Header("Specific Named Floors")]
    public List<FloorContentProfile> specialFloors;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public FloorContentProfile GetProfileForFloor(int floor, int runSeed)
    {
        FloorContentProfile special = specialFloors.Find(f => f.floorName == $"Floor {floor}");
        if (special != null)
            return special;

        List<FloorContentProfile> pool;
        if (floor < 3) pool = easyFloors;
        else if (floor < 6) pool = midFloors;
        else pool = hardFloors;

        return GetWeightedRandomFloor(pool, runSeed, floor);
    }


    private FloorContentProfile GetWeightedRandomFloor(List<FloorContentProfile> pool, int runSeed, int floorIndex)
    {
        if (pool == null || pool.Count == 0) return null;

        int seed = runSeed + floorIndex * 1000;
        System.Random rng = new System.Random(seed);

        int totalWeight = 0;
        foreach (var f in pool)
            totalWeight += f.weight;

        int roll = rng.Next(0, totalWeight);
        int runningSum = 0;

        foreach (var f in pool)
        {
            runningSum += f.weight;
            if (roll < runningSum)
                return f;
        }

        return pool[0];
    }
}