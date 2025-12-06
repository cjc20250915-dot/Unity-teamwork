using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("背景音乐")]
    public AudioClip bgm;

    private AudioSource audioSource;

    void Awake()
    {
        // 单例处理，确保全局唯一
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 切场景不销毁
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 创建音源组件
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = bgm;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        audioSource.Play();
    }
}
