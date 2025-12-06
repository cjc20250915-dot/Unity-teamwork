using UnityEngine;

// 当与其他车辆碰撞时上报 GameManager 并销毁双方。
public class CarCollisionHandler : MonoBehaviour
{
    [Header("2D 爆炸特效（Prefab）")]
    public GameObject explosion2D;

    bool hasHandled = false;

    void OnCollisionEnter(Collision collision)
    {
        if (hasHandled) return;

        // 只关心与其它带 Tag "Car" 的物体的碰撞
        if (collision.collider.CompareTag("Car"))
        {
            hasHandled = true;

            // === 生成 2D 卡通爆炸特效 ===
            if (explosion2D != null)
            {
                // 取碰撞接触点作为爆炸中心
                Vector3 pos = collision.contacts[0].point;

                // 让 2D 特效面向摄像机（billboard）
                Quaternion rot = Quaternion.LookRotation(Camera.main.transform.forward);

                GameObject fx = Instantiate(explosion2D, pos, rot);

                // 自动销毁（如果特效本身没有自动清除）
                Destroy(fx, 2f);
            }

            // 上报撞车事件 
            TrafficGameController.Instance?.OnCarCrash();

            // 销毁双方车辆（稍微延迟一点，保证特效能出现）
            Destroy(collision.gameObject, 0.05f);
            Destroy(gameObject, 0.05f);
        }
    }
}
