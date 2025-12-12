using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Background music")]
    public AudioClip bgm;

    private AudioSource audioSource;

    void Awake()
    {
        // Singleton handling to ensure global uniqueness
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Do not destroy when switching scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Create audio source component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = bgm;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        audioSource.Play();
    }
}
