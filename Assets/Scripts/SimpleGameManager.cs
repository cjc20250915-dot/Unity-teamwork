using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SimpleGameManager : MonoBehaviour
{
    public static SimpleGameManager Instance;
    public Text scoreText;
    public GameObject gameOverPanel;
    public int collisionsToLose = 1;
    
    int collisions = 0;
    float score = 0f;
    bool isGameOver = false;
    bool isPaused = false;
    
    // 属性访问器
    public bool IsGameOver => isGameOver;
    public bool IsPaused => isPaused;
    
    // 事件委托
    public delegate void PauseEventHandler(bool isPaused);
    public event PauseEventHandler OnPauseStateChanged;

    void Awake()
    {
        Debug.Log("SimpleGameManager.Awake() 开始");
        
        if (Instance == null) 
        {
            Instance = this;
            Time.timeScale = 1f;
            Debug.Log("SimpleGameManager实例已创建，Time.timeScale = 1");
        }
        else 
        {
            Debug.Log("已存在SimpleGameManager实例，销毁新实例");
            Destroy(gameObject);
        }
        
        Debug.Log("SimpleGameManager.Awake() 结束");
    }
    
    void Start()
    {
        Debug.Log("SimpleGameManager.Start() 被调用");
        Debug.Log($"当前暂停状态: {isPaused}");
        Debug.Log($"当前时间缩放: {Time.timeScale}");
    }
    
    void Update()
    {
        if (isGameOver || isPaused) return;
        
        score += Time.deltaTime;
        if (scoreText) 
            scoreText.text = $"Score: {Mathf.FloorToInt(score)}";
        
        // 快捷键支持：P键或ESC键暂停/恢复
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("检测到键盘快捷键，调用TogglePause");
            TogglePause();
        }
    }

    public void ReportCollision()
    {
        collisions++;
        if (collisions >= collisionsToLose) 
            GameOver();
    }

    public void GameOver()
    {
        isGameOver = true;
        if (gameOverPanel) 
            gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    
    public void TogglePause()
    {
        Debug.Log("=== TogglePause() 被调用 ===");
        Debug.Log($"当前暂停状态: {isPaused}");
        Debug.Log($"当前时间缩放: {Time.timeScale}");
        
        if (isGameOver) 
        {
            Debug.Log("游戏已结束，不处理暂停");
            return;
        }
        
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        
        Debug.Log($"切换后暂停状态: {isPaused}");
        Debug.Log($"切换后时间缩放: {Time.timeScale}");
        
        // 触发事件通知UI更新
        if (OnPauseStateChanged != null)
        {
            Debug.Log($"触发OnPauseStateChanged事件，有 {OnPauseStateChanged.GetInvocationList().Length} 个订阅者");
            OnPauseStateChanged?.Invoke(isPaused);
        }
        else
        {
            Debug.LogWarning("OnPauseStateChanged事件没有订阅者!");
        }
    }
    
    public void RestartGame()
    {
        Debug.Log("重新开始游戏");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}