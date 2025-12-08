using UnityEngine;

// 当与其他车辆碰撞时上报 GameManager 并销毁双方。
public class CarCollisionHandler : MonoBehaviour
{
    [Header("2D 爆炸特效（Prefab）")]
    public GameObject explosion2D;

    [Header("爆炸音效")]
    public AudioClip explosionSFX;     // 新增：爆炸音频
    private AudioSource audioSource;   // 新增：用于播放音效

    [Header("特效向摄像机偏移量")]
    public float cameraOffset = 0.3f;  // 新增：向摄像机方向偏移距离

    bool hasHandled = false;

    void Awake()
    {
        // 自动添加 AudioSource（如果预制体没有的话）
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f;   // 3D 声音（可听到位置）
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasHandled) return;

        // 只关心与其它带 Tag "Car" 的物体的碰撞
        if (collision.collider.CompareTag("Car"))
        {
            hasHandled = true;

            // ===== 播放爆炸音效 =====
            if (explosionSFX != null)
            {
                audioSource.PlayOneShot(explosionSFX);
            }

            // ===== 生成 2D 卡通爆炸特效 =====
            if (explosion2D != null)
            {
                Vector3 pos = collision.contacts[0].point;

                // 计算偏移：从碰撞点向摄像机方向靠近一点
                Vector3 camDir = (Camera.main.transform.position - pos).normalized;
                pos += camDir * cameraOffset;

                // billboard 朝向摄像机
                Quaternion rot = Quaternion.LookRotation(Camera.main.transform.forward);

                GameObject fx = Instantiate(explosion2D, pos, rot);
                Destroy(fx, 2f);
            }

            // ===== 上报撞车事件 =====
            TrafficGameController.Instance?.OnCarCrash();

            // ===== 销毁双方车辆（稍微延迟一点，保证音效与特效能播放） =====
            Destroy(collision.gameObject, 0.05f);
            Destroy(gameObject, 0.05f);
        }
    }
}
