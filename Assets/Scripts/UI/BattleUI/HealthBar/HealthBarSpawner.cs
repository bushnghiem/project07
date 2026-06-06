using UnityEngine;

public class WorldHealthBarSpawner : MonoBehaviour
{
    [SerializeField]
    private HealthBarUI healthBarPrefab;

    private void Start()
    {
        HealthComponent health =
            GetComponent<HealthComponent>();

        if (health == null)
        {
            Debug.LogError($"No HealthComponent on {gameObject.name}");
            return;
        }

        Debug.Log($"Spawning health bar for {gameObject.name}");

        HealthBarUI bar =
            Instantiate(healthBarPrefab);

        bar.Initialize(
            health,
            transform);
    }
}