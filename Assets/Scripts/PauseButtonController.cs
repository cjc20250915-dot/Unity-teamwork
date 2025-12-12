using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseButtonController : MonoBehaviour
{
    [Header("Button References")]
    [SerializeField] private Button pauseButton;
    
    [Header("Icon Sprites")]
    [SerializeField] private Sprite pauseSprite;    // GUI_34
    [SerializeField] private Sprite playSprite;     // GUI_33
    
    [Header("Animation")]
    [SerializeField] private float scaleDuration = 0.2f;
    [SerializeField] private float pressedScale = 0.9f;
    
    private Vector3 originalScale;
    private Coroutine scaleCoroutine;
    
    void Start()
    {
        Debug.Log("PauseButtonController.Start() 开始");

        // Get component reference
        if (pauseButton == null)
            pauseButton = GetComponent<Button>();
        
        Debug.Log($"找到Button组件: {pauseButton != null}");
        
        if (pauseButton != null)
        {
            Debug.Log($"Button的Image组件: {pauseButton.image != null}");
            Debug.Log($"当前Source Image: {pauseButton.image?.sprite?.name}");
        }

        // Check Sprite references
        Debug.Log($"暂停Sprite (GUI_34): {pauseSprite != null} - {pauseSprite?.name}");
        Debug.Log($"播放Sprite (GUI_33): {playSprite != null} - {playSprite?.name}");

        // Save original scaling
        originalScale = transform.localScale;

        // Add button click listener
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        Debug.Log("已添加按钮点击监听");

        // Initialize icon
        UpdatePauseButtonIcon();

        // Listen for changes in the game's paused state
        if (SimpleGameManager.Instance != null)
        {
            SimpleGameManager.Instance.OnPauseStateChanged += OnGamePaused;
            Debug.Log("已监听SimpleGameManager事件");
        }
        else
        {
            Debug.LogWarning("PauseButtonController: SimpleGameManager.Instance 为 null");
        }
        
        Debug.Log("PauseButtonController.Start() 结束");
    }
    
    void OnDestroy()
    {
        if (SimpleGameManager.Instance != null)
        {
            SimpleGameManager.Instance.OnPauseStateChanged -= OnGamePaused;
        }
    }
    
    void OnPauseButtonClicked()
    {
        Debug.Log("暂停按钮被点击");

        // Play click animation
        PlayButtonAnimation();

        // Toggle pause state
        if (SimpleGameManager.Instance != null)
        {
            Debug.Log("调用SimpleGameManager.TogglePause()");
            SimpleGameManager.Instance.TogglePause();
        }
        else
        {
            Debug.LogError("SimpleGameManager.Instance 为 null!");
        }
    }
    
    void UpdatePauseButtonIcon()
    {
        Debug.Log("UpdatePauseButtonIcon() 被调用");
        
        if (pauseButton == null || pauseButton.image == null)
        {
            Debug.LogWarning("PauseButtonController: Button或Image组件未找到");
            return;
        }

        // Update icon based on game pause status
        bool isPaused = SimpleGameManager.Instance != null && SimpleGameManager.Instance.IsPaused;
        Debug.Log($"当前暂停状态: {isPaused}");

        // Note: Button.image is the Image component in the Button component that displays the Source Image.
        if (isPaused && playSprite != null)
        {
            Debug.Log($"设置为播放图标: {playSprite.name} (GUI_33)");
            pauseButton.image.sprite = playSprite;  // Display a triangle when paused (GUI_33)
        }
        else if (!isPaused && pauseSprite != null)
        {
            Debug.Log($"设置为暂停图标: {pauseSprite.name} (GUI_34)");
            pauseButton.image.sprite = pauseSprite; // Displays two vertical bars at runtime (GUI_34)
        }
        else
        {
            Debug.LogWarning("无法更新图标: Sprite引用为空");
        }
    }
    
    void OnGamePaused(bool isPaused)
    {
        Debug.Log($"OnGamePaused事件触发: isPaused = {isPaused}");
        UpdatePauseButtonIcon();
    }
    
    void PlayButtonAnimation()
    {
        Debug.Log("播放按钮动画");
        
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);
        
        scaleCoroutine = StartCoroutine(ButtonScaleAnimation());
    }
    
    IEnumerator ButtonScaleAnimation()
    {
        // Press effect
        float elapsed = 0f;
        while (elapsed < scaleDuration / 2)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / (scaleDuration / 2);
            transform.localScale = Vector3.Lerp(originalScale, originalScale * pressedScale, t);
            yield return null;
        }

        // Bounce effect
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
    
    void Update()
    {

    }
}