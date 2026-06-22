using UnityEngine;

public class ProjectileVisualController : MonoBehaviour
{
    [SerializeField] private Transform visualRoot;

    private GameObject currentVisual;

    public void ApplyVisuals(ProjectileVisualData visualData)
    {
        if (visualData == null)
            return;

        if (currentVisual != null)
            Destroy(currentVisual);

        currentVisual = Instantiate(
            visualData.visualPrefab,
            visualRoot
        );

        currentVisual.transform.localPosition = Vector3.zero;
        currentVisual.transform.localRotation = Quaternion.identity;

        currentVisual.transform.localScale =
            Vector3.one * visualData.visualScale;

        if (visualData.overrideMaterial != null)
        {
            foreach (var renderer in currentVisual.GetComponentsInChildren<Renderer>())
            {
                renderer.material =
                    visualData.overrideMaterial;
            }
        }
    }

    public void HideVisuals()
    {
        if (currentVisual != null)
            currentVisual.SetActive(false);
    }
}