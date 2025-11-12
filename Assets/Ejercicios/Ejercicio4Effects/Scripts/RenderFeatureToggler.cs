using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Reflection;

public class RenderFeatureToggler : MonoBehaviour
{
    private UniversalRendererData rendererData;

    void Awake()
    {
        rendererData = GetRendererData();
        if (rendererData == null)
            Debug.LogError("⚠️ Could not find UniversalRendererData. Make sure your project uses URP and the active renderer is Universal Renderer.");
    }

    // --- PUBLIC API ---
    public void ToggleFullScreen(bool active)
    {
        ToggleFeature<FullScreenPassRendererFeature>(active);
    }

    public void ToggleRenderRed(bool active)
    {
        // Use the exact name shown in the Renderer Features list
        ToggleFeature("Render Red", active);
    }
    // --- CORE ---
    private void ToggleFeature<T>(bool active) where T : ScriptableRendererFeature
    {
        if (rendererData == null)
        {
            Debug.LogError("RendererData is null!");
            return;
        }

        foreach (var feature in rendererData.rendererFeatures)
        {
            if (feature is T)
            {
                feature.SetActive(active);
                rendererData.SetDirty(); // force refresh
                Debug.Log($"✅ {feature.name} ({typeof(T).Name}) set to {(active ? "ENABLED" : "DISABLED")}");
                return;
            }
        }

        Debug.LogWarning($"Feature of type {typeof(T).Name} not found!");
    }
    // 🔘 Toggle by Feature Name (useful for multiple RenderObjects)
    public void ToggleFeature(string featureName, bool active)
    {
        if (rendererData == null) return;

        foreach (var feature in rendererData.rendererFeatures)
        {
            if (feature.name == featureName)
            {
                feature.SetActive(active);
                rendererData.SetDirty();
                Debug.Log($"✅ Feature '{featureName}' set to {(active ? "ENABLED" : "DISABLED")}");
                return;
            }
        }

        Debug.LogWarning($"Feature named '{featureName}' not found!");
    }

    // --- SAFE RENDERERDATA RETRIEVAL ---
    private UniversalRendererData GetRendererData()
    {
        var urpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        if (urpAsset == null)
        {
            Debug.LogError("Current render pipeline is not a Universal Render Pipeline Asset.");
            return null;
        }

        // Access internal "m_RendererDataList"
        var field = typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", BindingFlags.NonPublic | BindingFlags.Instance);
        if (field == null)
        {
            Debug.LogError("m_RendererDataList field not found via reflection (Unity may have changed internals).");
            return null;
        }

        var list = field.GetValue(urpAsset) as ScriptableRendererData[];
        if (list == null || list.Length == 0)
        {
            Debug.LogError("Renderer data list empty or null in URP asset.");
            return null;
        }

        // Fallback to first renderer (active by default)
        return list[0] as UniversalRendererData;
    }
}
