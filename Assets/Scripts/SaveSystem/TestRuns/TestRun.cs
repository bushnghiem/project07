using UnityEngine;

public class TestRun : MonoBehaviour
{
    public UnitSpawner unitSpawner;
    public EnvironmentSpawner environmentSpawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var runData = RunManager.Instance.CurrentRun;
        var encounterData = runData.currentFloorData.currentEncounter;

        MusicManager.PlayMusic(encounterData.music);

        unitSpawner.SetAnchorPositions(
            encounterData.playerAnchorPosition,
            encounterData.enemyAnchorPosition
        );

        if (encounterData.playerFormation != null)
        {
            unitSpawner.SpawnPlayerTeam(
                runData.team,
                encounterData.playerFormation
            );
        }
        else
        {
            unitSpawner.SpawnPlayerTeam(
                runData.team,
                runData.playerFormation
            );
        }

        unitSpawner.SpawnEnemyTeam(
            encounterData.enemies,
            encounterData.enemyFormation
        );

        if (encounterData.environmentLayout != null)
        {
            Vector3 combatCenter =
                (encounterData.playerAnchorPosition +
                 encounterData.enemyAnchorPosition) * 0.5f;
            Debug.Log(runData.currentFloorData.currentEncounterSeed);
            environmentSpawner.SpawnEnvironment(
                encounterData.environmentLayout,
                combatCenter,
                runData.currentFloorData.currentEncounterSeed
            );
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
