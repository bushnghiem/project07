using UnityEngine;

public class TestRun : MonoBehaviour
{
    public UnitSpawner spawner;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RunData runData = RunManager.Instance.CurrentRun;
        spawner.SpawnTeam(runData);
        spawner.SpawnUnit(spawner.enemyData, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
