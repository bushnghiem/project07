using UnityEngine;

public class TestRun : MonoBehaviour
{
    public UnitSpawner spawner;
    public FormationData playerFormation;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var runData = RunManager.Instance.CurrentRun;
        var encounterData = runData.currentEncounter;

        spawner.SpawnPlayerTeam(runData.team, playerFormation);
        spawner.SpawnEnemyTeam(encounterData.enemies, encounterData.enemyFormation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
