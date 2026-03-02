using UnityEngine;

public class TestRun : MonoBehaviour
{
    public UnitSpawner unitSpawner;
    public EnvironmentSpawner environmentSpawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var runData = RunManager.Instance.CurrentRun;
        var encounterData = runData.currentEncounter;

        unitSpawner.SetAnchorPositions(encounterData.playerAnchorPosition, encounterData.enemyAnchorPosition);

        if (encounterData.playerFormation != null)
        {
            unitSpawner.SpawnPlayerTeam(runData.team, encounterData.playerFormation);
        }
        else
        {
            unitSpawner.SpawnPlayerTeam(runData.team, runData.playerFormation);
        }

        unitSpawner.SpawnEnemyTeam(encounterData.enemies, encounterData.enemyFormation);
        if (encounterData.environmentLayout != null)
        {
            environmentSpawner.SpawnEnvironment(encounterData.environmentLayout);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
