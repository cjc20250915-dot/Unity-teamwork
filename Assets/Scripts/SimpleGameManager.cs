using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleGameManager : MonoBehaviour
{
    public static SimpleGameManager Instance;

    bool isPaused = false;

    
    public bool IsPaused => isPaused;

    // Pause events (e.g., pause menu UI)
    public delegate void PauseEventHandler(bool isPaused);
    public event PauseEventHandler OnPauseStateChanged;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Time.timeScale = 1f;   // Ensure the game starts at normal speed
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Update()
    {
        // Supports P/ESC switching to pause
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }



    // Toggle between pause and resume

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        // Notify UI
        OnPauseStateChanged?.Invoke(isPaused);
    }


    // Reload the current scene for the Restart button

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
