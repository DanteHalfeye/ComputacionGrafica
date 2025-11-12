using UnityEngine;

public class Toggler : MonoBehaviour
{
    private bool featureEnable;
    public void ToggleFeature()
    {
        featureEnable = !featureEnable;
        var toggler = FindObjectOfType<RenderFeatureToggler>();
        
// Enable fullscreen pass
        toggler.ToggleFullScreen(featureEnable);

// Disable red render (RenderObjects)
        toggler.ToggleRenderRed(featureEnable);
        
    }
}
