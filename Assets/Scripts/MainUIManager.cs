using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject mainPanel;         // 主面板（Start/About/Quit按钮）
    public GameObject aboutPanel;        // About规则弹窗
    public GameObject quitPanel;         // Quit确认弹窗

    void Start()
    {
        mainPanel.SetActive(true);
        aboutPanel.SetActive(false);
        quitPanel.SetActive(false);
    }

    // Start按钮：跳转到LevelUI场景（无改动）
    public void OnStartButtonClick()
    {
        ChangeScene("LevelUI");
    }

    // 【修改1】About按钮：只显示弹窗，不隐藏主面板
    public void OnAboutButtonClick()
    {
        // 删掉 mainPanel.SetActive(false);
        aboutPanel.SetActive(true); // 仅显示弹窗
    }

    // 【修改2】About弹窗关闭按钮：只隐藏弹窗（主面板本就显示，无需再激活）
    public void OnAboutQuitButtonClick()
    {
        aboutPanel.SetActive(false);
        // 删掉 mainPanel.SetActive(true);
    }

    // 【修改3】Quit按钮：只显示弹窗，不隐藏主面板
    public void OnQuitButtonClick()
    {
        // 删掉 mainPanel.SetActive(false);
        quitPanel.SetActive(true); // 仅显示弹窗
    }

    // 【修改4】Quit弹窗Yes按钮：退出游戏（无改动）
    public void OnQuitYesButtonClick()
    {
        Debug.Log("游戏已退出！");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // 【修改5】Quit弹窗No按钮：只隐藏弹窗（主面板本就显示）
    public void OnQuitNoButtonClick()
    {
        quitPanel.SetActive(false);
        // 删掉 mainPanel.SetActive(true);
    }

    // 通用场景切换方法（无改动）
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}