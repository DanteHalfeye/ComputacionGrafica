using UnityEngine;
public class DistortEffect: MonoBehaviour
{
    [Tooltip("Material to control (instance, not shared)")]
    public Material targetMaterial;

    [Tooltip("Shader property name, e.g. _RippleStrength")]
    public string propertyName = "_MyValue";

    [Tooltip("Range of values to lerp between")]
    public float from = 0f;
    public float to = 1f;

    [Tooltip("Time (seconds) for one full cycle")]
    public float duration = 2f;

    [Tooltip("Loop the animation")]
    public bool loop = true;

    private int propertyID;

    private void OnEnable()
    {
        if (targetMaterial != null)
            propertyID = Shader.PropertyToID(propertyName);
    }

    void Update()
    {
        if (targetMaterial == null) return;

        float t = Mathf.PingPong(Time.time / duration, 1f);
        float value = Mathf.Lerp(from, to, t);
        targetMaterial.SetFloat(propertyID, value);
    }
}