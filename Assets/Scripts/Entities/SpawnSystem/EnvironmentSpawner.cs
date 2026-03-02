using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    public void SpawnEnvironment(EnvironmentLayout layout)
    {
        foreach (var objData in layout.environmentObjects)
        {
            GameObject obj = Instantiate(
                objData.prefab,
                objData.position,
                Quaternion.Euler(objData.rotation)
            );

            obj.transform.localScale = objData.scale;
        }
    }
}
