using UnityEngine;

public class DeathManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        DeathEvent.OnEntityDeath += HandleDeath;
    }

    private void OnDisable()
    {
        DeathEvent.OnEntityDeath -= HandleDeath;
    }

    public void HandleDeath(Vector3 deathPosition, GameObject gameObject)
    {
        Debug.Log(gameObject.name + "death occured at: " + deathPosition);
        Destroy(gameObject);
    }
}
