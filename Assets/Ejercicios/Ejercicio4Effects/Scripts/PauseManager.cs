using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    [Header("Optional Events")]
    public UnityEvent OnPause;
    public UnityEvent OnResume;

    [Header("UI Menu (optional)")]
    [SerializeField] private GameObject pauseMenuUI;

    private void Awake()
    {
        if (pauseMenuUI)
            pauseMenuUI.SetActive(false);
    }

    /// <summary>
    /// Toggles pause on/off.
    /// You can call this from a UI button.
    /// </summary>
    public void TogglePause()
    {
        if (IsPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        IsPaused = true;

        if (pauseMenuUI)
            pauseMenuUI.SetActive(true);

        OnPause?.Invoke();
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        IsPaused = false;

        if (pauseMenuUI)
            pauseMenuUI.SetActive(false);

        OnResume?.Invoke();
    }
}