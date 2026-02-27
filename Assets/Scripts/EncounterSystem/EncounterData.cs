using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/Encounter Data")]
public class EncounterData : ScriptableObject
{
    public string encounterName;
    public List<ShipRunData> enemies;
    public FormationData enemyFormation;

    public bool isElite;
    public bool isBoss;

    public int runCurrencyReward;
    public int metaCurrencyReward;
}
