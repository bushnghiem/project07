using UnityEngine;

public class ShipVisualController : MonoBehaviour
{
    [SerializeField] private Transform visualRoot;

    private GameObject currentVisual;

    public void ApplyVisuals(ShipVisualData visualData)
    {
        if (visualData == null)
        {
            Debug.LogWarning("No visual data assigned.");
            return;
        }

        if (currentVisual != null)
        {
            Destroy(currentVisual);
        }

        currentVisual = Instantiate(
            visualData.visualPrefab,
            visualRoot
        );

        currentVisual.transform.localPosition = Vector3.zero;
        currentVisual.transform.localRotation = Quaternion.identity;

        if (visualData.overrideMaterial != null)
        {
            foreach (var renderer in currentVisual.GetComponentsInChildren<Renderer>())
            {
                renderer.material = visualData.overrideMaterial;
            }
        }
    }

    public void HideVisuals()
    {
        if (currentVisual != null)
        {
            currentVisual.SetActive(false);
        }
    }

    public void DestroyVisuals()
    {
        if (currentVisual != null)
        {
            Destroy(currentVisual);
            currentVisual = null;
        }
    }

    public void OnDeath()
    {
        HideVisuals();
    }

}