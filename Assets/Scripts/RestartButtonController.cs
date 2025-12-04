using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RestartButtonController : MonoBehaviour
{
    [Header("Button References")]
    [SerializeField] private Button restartButton;
    
    [Header("Animation")]
    [SerializeField] private float scaleDuration = 0.2f;
    [SerializeField] private float pressedScale = 0.9f;
    
    private Vector3 originalScale;
    private Coroutine scaleCoroutine;
    
    void Start()
    {
        // 获取组件引用
        if (restartButton == null)
            restartButton = GetComponent<Button>();
        
        // 保存原始缩放
        originalScale = transform.localScale;
        
        // 添加按钮点击监听
        restartButton.onClick.AddListener(OnRestartButtonClicked);
    }
    
    void OnRestartButtonClicked()
    {
        // 播放点击动画
        PlayButtonAnimation();
        
        // 延迟执行重新开始，让动画有播放时间
        Invoke("ExecuteRestart", scaleDuration);
    }
    
    void ExecuteRestart()
    {
        if (SimpleGameManager.Instance != null)
        {
            SimpleGameManager.Instance.RestartGame();
        }
        else
        {
            // 如果没有GameManager，直接重新加载场景
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
        }
    }
    
    void PlayButtonAnimation()
    {
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);
        
        scaleCoroutine = StartCoroutine(ButtonScaleAnimation());
    }
    
    IEnumerator ButtonScaleAnimation()
    {
        // 按下效果
        float elapsed = 0f;
        while (elapsed < scaleDuration / 2)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / (scaleDuration / 2);
            transform.localScale = Vector3.Lerp(originalScale, originalScale * pressedScale, t);
            yield return null;
        }
        
        // 弹起效果
        elapsed = 0f;
        while (elapsed < scaleDuration / 2)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / (scaleDuration / 2);
            transform.localScale = Vector3.Lerp(originalScale * pressedScale, originalScale, t);
            yield return null;
        }
        
        transform.localScale = originalScale;
    }
}