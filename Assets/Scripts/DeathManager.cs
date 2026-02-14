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

    public void HandleDeath(Entity entity)
    {
        Debug.Log(entity + "death occured at: " + entity.Position);
        entity.Kill();
    }
}
