using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Effect
{
    public string name;
    public UnityEvent onActivate;
    public UnityEvent onDeactivate;
    [HideInInspector] public bool isPlaying;
}

public class EffectManager : MonoBehaviour
{
    [Header("Available Effects")]
    public List<Effect> effects = new List<Effect>();

    private Effect currentEffect;
    private Effect newEffect = null;
    private int currentIndex = -1; // Track index in the list

    /// <summary>
    /// Selects an effect by name (does not play it yet)
    /// </summary>
    private void Awake()
    {
        currentEffect = effects[1];
    }

    public void SelectEffect(string effectName)
    {
        newEffect = effects.Find(e => e.name == effectName);

        if (newEffect == null)
        {
            Debug.LogWarning($"Effect '{effectName}' not found!");
            return;
        }
    }

    /// <summary>
    /// Plays the selected effect
    /// </summary>
    public void PlayEffect()
    {
        // Stop current if needed
        if (currentEffect != null && currentEffect != newEffect)
        {
            StopEffect(currentEffect.name);
        }

        if (newEffect == null)
        {
            newEffect = effects[1];
            Debug.LogWarning("No effect selected to play!");
        }

        // Prevent reactivation if already active
        if (newEffect.isPlaying)
        {
            Debug.Log($"Effect '{newEffect.name}' is already playing.");
            return;
        }

        newEffect.isPlaying = true;
        currentEffect = newEffect;
        newEffect.onActivate.Invoke();
        Debug.Log($"Activated effect: {newEffect.name}");
    }

    /// <summary>
    /// Stops an effect by name
    /// </summary>
    public void StopEffect(string effectName)
    {
        Effect effect = effects.Find(e => e.name == effectName);

        if (effect == null || !effect.isPlaying) return;

        effect.isPlaying = false;
        effect.onDeactivate.Invoke();

        if (currentEffect == effect)
            currentEffect = null;

        Debug.Log($"Deactivated effect: {effectName}");
    }

    /// <summary>
    /// Stops all effects
    /// </summary>
    public void StopAllEffects()
    {
        foreach (var e in effects)
        {
            if (e.isPlaying)
            {
                e.isPlaying = false;
                e.onDeactivate.Invoke();
            }
        }

        currentEffect = null;
        Debug.Log("All effects deactivated.");
    }

    /// <summary>
    /// Cycles to the next effect in the list.
    /// Automatically stops the current one and activates the next.
    /// </summary>
    public void NextEffect()
    {
        if (effects.Count == 0)
        {
            Debug.LogWarning("No effects available to cycle through!");
            return;
        }

        // Stop current effect
        if (currentEffect != null)
        {
            StopEffect(currentEffect.name);
        }

        // Move to next index (looping back to start)
        currentIndex = (currentIndex + 1) % effects.Count;
        newEffect = effects[currentIndex];

        Debug.Log($"Switching to next effect: {newEffect.name}");
        PlayEffect();
    }
}
