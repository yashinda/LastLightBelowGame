using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FogController : MonoBehaviour
{
    [SerializeField] private UniversalRendererData rendererData;
    [SerializeField] private bool enableFogOnStart = true;

    private ScriptableRendererFeature volumetricFogFeature;

    private void Start()
    {
        volumetricFogFeature = rendererData.rendererFeatures.Find(feature => feature.name == "Volumetric Fog");

        if (volumetricFogFeature != null)
        {
            SetFog(enableFogOnStart);
        }
        else
        {
            Debug.LogError("Renderer Feature 'Volumetric Fog' не найден.");
        }
    }
    
    private void SetFog(bool enabled)
    {
        volumetricFogFeature.SetActive(enabled);
    }
}