using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleGameManager : MonoBehaviour
{
    public static SimpleGameManager Instance;

    bool isPaused = false;

    // 属性访问器
    public bool IsPaused => isPaused;

    // 暂停事件（例如暂停菜单 UI）
    public delegate void PauseEventHandler(bool isPaused);
    public event PauseEventHandler OnPauseStateChanged;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Time.timeScale = 1f;   // 确保游戏开始时是正常速度
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Update()
    {
        // 支持 P / ESC 切换暂停
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }


    /// <summary>
    /// 切换暂停 / 继续
    /// </summary>
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        // 通知 UI
        OnPauseStateChanged?.Invoke(isPaused);
    }


    /// <summary>
    /// 重新加载当前场景，用于 Restart 按钮
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
