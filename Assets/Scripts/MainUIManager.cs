using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject mainPanel;         // Main panel (Start/About/Quit buttons)
    public GameObject aboutPanel;        // About Rules Pop-up
    public GameObject quitPanel;         // Quit confirmation pop-up

    void Start()
    {
        mainPanel.SetActive(true);
        aboutPanel.SetActive(false);
        quitPanel.SetActive(false);
    }

    // Start button: Jump to the LevelUI scene
    public void OnStartButtonClick()
    {
        ChangeScene("LevelUI");
    }

    // About button: Only show the pop-up, do not hide the main panel.
    public void OnAboutButtonClick()
    {
        // del mainPanel.SetActive(false);
        aboutPanel.SetActive(true); // Show only pop-ups
    }

    // About pop-up close button: Only hides the pop-up (it's already displayed on the main panel, no need to activate it again).
    public void OnAboutQuitButtonClick()
    {
        aboutPanel.SetActive(false);
        // del mainPanel.SetActive(true);
    }

    // Quit button: Only show the pop-up window, do not hide the main panel.
    public void OnQuitButtonClick()
    {
        // del mainPanel.SetActive(false);
        quitPanel.SetActive(true); // Show only pop-ups
    }

    // Quit pop-up Yes button: Exit game
    public void OnQuitYesButtonClick()
    {
        Debug.Log("游戏已退出！");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Quit pop-up No button: Only hides the pop-up (it's already displayed on the main panel).
    public void OnQuitNoButtonClick()
    {
        quitPanel.SetActive(false);
        // del mainPanel.SetActive(true);
    }

    // General scene switching method
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}