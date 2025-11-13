using UnityEngine;
using System.Collections;

public class SmoothFullscreenValue : MonoBehaviour
{
    [Header("Target Material")]
    [SerializeField] private Material fullscreenMaterial;

    [Header("Material Property")]
    [SerializeField] private string propertyName = "_Lerp";
    [SerializeField] private float defaultDuration = 1f;

    [Header("Default Values (for Unity Events)")]
    [SerializeField] private float defaultTargetValue = 1f;
    [SerializeField] private float defaultStartValue = 0f;
    [SerializeField] private float defaultPingPongTargetValue = 1f;

    private float currentValue;
    private Coroutine currentRoutine;

    private void Awake()
    {
        if (fullscreenMaterial == null)
        {
            Debug.LogError("No fullscreen material assigned!", this);
            enabled = false;
            return;
        }

        currentValue = fullscreenMaterial.GetFloat(propertyName);
    }

    // ----------- PUBLIC METHODS -----------

    /// <summary>
    /// Smoothly transitions to a value and stays there.
    /// </summary>
    public void SetValueSmooth(float targetValue)
    {
        SetValueSmooth(targetValue, defaultDuration);
    }

    public void SetValueHarsh(float targetValue)
    {
        fullscreenMaterial.SetFloat(propertyName, targetValue);
    }

    public void SetValueSmooth(float targetValue, float duration)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(SmoothChange(targetValue, duration));
    }

    /// <summary>
    /// Smoothly transitions from startValue to targetValue and then back.
    /// </summary>
    public void SetValueSmoothPingPong(float startValue, float targetValue)
    {
        SetValueSmoothPingPong(startValue, targetValue, defaultDuration);
    }

    public void SetValueSmoothPingPong(float startValue, float targetValue, float duration)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(SmoothChangePingPong(startValue, targetValue, duration));
    }

    // ----------- UNITY EVENT SHORTCUTS -----------

    // For UnityEvent (uses default serialized values)
    public void TriggerSmooth()
    {
        SetValueSmooth(defaultTargetValue, defaultDuration);
    }

    // For UnityEvent (PingPong version)
    public void TriggerPingPong()
    {
        SetValueSmoothPingPong(defaultStartValue, defaultPingPongTargetValue, defaultDuration);
    }

    // ----------- INTERNAL COROUTINES -----------

    private IEnumerator SmoothChange(float targetValue, float duration)
    {
        float startValue = currentValue;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            currentValue = Mathf.Lerp(startValue, targetValue, t);
            fullscreenMaterial.SetFloat(propertyName, currentValue);
            yield return null;
        }

        currentValue = targetValue;
        fullscreenMaterial.SetFloat(propertyName, currentValue);
    }

    private IEnumerator SmoothChangePingPong(float startValue, float targetValue, float duration)
    {
        // Forward
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            currentValue = Mathf.Lerp(startValue, targetValue, t);
            fullscreenMaterial.SetFloat(propertyName, currentValue);
            yield return null;
        }

        // Backward
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            currentValue = Mathf.Lerp(targetValue, startValue, t);
            fullscreenMaterial.SetFloat(propertyName, currentValue);
            yield return null;
        }

        currentValue = startValue;
        fullscreenMaterial.SetFloat(propertyName, currentValue);
    }
}
