using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/Encounter Data")]
public class EncounterData : ScriptableObject
{
    public string encounterName;
    public List<ShipRunData> enemies;
    public FormationData enemyFormation;
    public FormationData playerFormation;
    public EnvironmentLayout environmentLayout;

    public Vector3 playerAnchorPosition;
    public Vector3 enemyAnchorPosition;

    public EncounterType encounterType;

    public int runCurrencyReward;
    public int metaCurrencyReward;
}
