using UnityEngine;

/// <summary>
/// 当与其他车辆碰撞时上报 GameManager 并销毁双方。
/// 文件名必须为 CarCollisionHandler.cs，类名必须为 CarCollisionHandler（区分大小写）。
/// </summary>
public class CarCollisionHandler : MonoBehaviour
{
    // 可选：用于避免重复处理同一次碰撞
    bool hasHandled = false;

    void OnCollisionEnter(Collision collision)
    {
        if (hasHandled) return;

        // 只关心与其它带 Tag "Car" 的物体的碰撞
        if (collision.collider.CompareTag("Car"))
        {
            hasHandled = true;

            // 上报 GameManager（如果有）
            if (SimpleGameManager.Instance != null)
            {
                SimpleGameManager.Instance.ReportCollision();
            }

            // 可选：播放 VFX / 声音

            // 销毁双方（延迟一点让效果能播放）
            Destroy(collision.gameObject, 0.1f);
            Destroy(gameObject, 0.1f);
        }
    }
}
