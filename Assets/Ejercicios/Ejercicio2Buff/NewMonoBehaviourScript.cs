using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class MaterialFloatPlayableBehaviour : PlayableBehaviour
{
    public string propertyName;
    public float startValue;
    public float endValue;
    public bool playOnce = true;
    public bool playReverse = false;

    private Material mat;
    private float originalValue;
    private bool originalStored = false;

    public void SetMaterial(Material material)
    {
        mat = material;
        if (mat != null && !string.IsNullOrEmpty(propertyName) && mat.HasProperty(propertyName))
        {
            originalValue = mat.GetFloat(propertyName);
            originalStored = true;
        }
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (mat == null || string.IsNullOrEmpty(propertyName) || !mat.HasProperty(propertyName))
            return;

        double duration = playable.GetDuration();
        double time = playable.GetTime();

        float normalizedT = 0f;
        if (duration > 1e-6)
        {
            float rawT = (float)(time / duration);

            if (playOnce)
                normalizedT = Mathf.Clamp01(rawT);
            else
                normalizedT = Mathf.PingPong(rawT, 1f);

            if (playReverse)
                normalizedT = 1f - normalizedT;
        }

        float value = Mathf.Lerp(startValue, endValue, normalizedT);
        mat.SetFloat(propertyName, value);
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (mat == null || string.IsNullOrEmpty(propertyName) || !mat.HasProperty(propertyName))
            return;

        if (playOnce)
        {
            // Keep the final value (respecting playReverse)
            float finalValue = playReverse ? startValue : endValue;
            mat.SetFloat(propertyName, finalValue);
        }
        else if (originalStored)
        {
            // Restore original if not "play once"
            mat.SetFloat(propertyName, originalValue);
        }
    }
}
