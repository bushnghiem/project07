using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/PersonalityProfile")]
public class EnemyAIPersonality : ScriptableObject
{
    [Header("Behavior Weights")]
    [Range(0f, 1f)] public float aggression = 0.7f;
    [Range(0f, 1f)] public float orbitPreference = 0.5f;
    [Range(0f, 1f)] public float ramPreference = 0.0f;

    [Header("Combat")]
    [Range(0f, 20f)] public float aimErrorAngle = 6f;
    public float preferredShootDistancePercent = 0.7f;
    public float ramDistanceThreshold = 3f;

    [Header("Decision Making")]
    [Range(0f, 1f)] public float decisiveness = 0.7f;
}