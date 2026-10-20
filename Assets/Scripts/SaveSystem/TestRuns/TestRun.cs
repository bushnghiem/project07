using UnityEngine;

public class TestRun : MonoBehaviour
{
    public UnitSpawner unitSpawner;
    public EnvironmentSpawner environmentSpawner;

    void Start()
    {
        var runData = RunManager.Instance.CurrentRun;
        var floorData = runData.currentFloorData;
        var encounterData = floorData.currentEncounter;

        if (encounterData == null)
        {
            Debug.LogError("No current encounter assigned!");
            return;
        }

        MusicManager.PlayMusic(encounterData.music);

        // Set the normal player and enemy anchor positions.
        unitSpawner.SetAnchorPositions(
            encounterData.playerAnchorPosition,
            encounterData.enemyAnchorPosition
        );

        // Spawn player team using the selected formation.
        FormationData playerFormation =
            encounterData.playerFormation != null
                ? encounterData.playerFormation
                : runData.playerFormation;

        unitSpawner.SpawnPlayerTeam(
            runData.team,
            playerFormation
        );

        // Calculate the center of the combat arena.
        Vector3 combatCenter =
            (encounterData.playerAnchorPosition +
             encounterData.enemyAnchorPosition) * 0.5f;

        // Use the saved encounter seed for deterministic spawning.
        int encounterSeed = floorData.currentEncounterSeed;

        Debug.Log($"Encounter Seed: {encounterSeed}");
        Debug.Log($"Enemy Spawn Mode: {encounterData.enemySpawnMode}");

        // Spawn enemies using the configured spawn mode.
        unitSpawner.SpawnEnemyTeam(
            encounterData.enemies,
            encounterData.enemyFormation,
            encounterData.enemySpawnMode,
            encounterData.enemySpawnSettings,
            combatCenter,
            encounterSeed
        );

        // Spawn the environment with the same encounter seed.
        if (encounterData.environmentLayout != null)
        {
            environmentSpawner.SpawnEnvironment(
                encounterData.environmentLayout,
                combatCenter,
                encounterSeed
            );
        }
    }
}