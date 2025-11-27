using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(WaypointFollower))]
public class CarSensor : MonoBehaviour
{
    public float sensorRadius = 0.5f;
    public float baseCheckDistance = 3f;
    public float reactionTime = 0.2f;
    public float brakingDeceleration = 8f;
    public LayerMask carLayer;
    public LayerMask intersectionLayer;

    WaypointFollower follower;
    Rigidbody rb;

    // --- 新增：停车锁 ---
    bool stopLocked = false;       // 进入检测区时锁定停车
    bool lightIsRed = false;       // 红灯状态
    bool carInFront = false;       // 前车状态

    void Awake()
    {
        follower = GetComponent<WaypointFollower>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);

        float reactionDistance = Mathf.Max(0f, forwardSpeed) * reactionTime;
        float brakingDistance = (forwardSpeed * forwardSpeed) / (2f * brakingDeceleration);
        float checkDistance = baseCheckDistance + reactionDistance + brakingDistance + 0.25f;

        Vector3 origin = transform.position + transform.forward * (sensorRadius + 0.1f) + Vector3.up * 0.5f;

        // -- 检测前车（SphereCast） --
        RaycastHit hitCar;
        bool carHit = Physics.SphereCast(origin, sensorRadius, transform.forward, out hitCar,
                                         checkDistance, carLayer, QueryTriggerInteraction.Ignore);

        // 清理：忽略自己
        if (carHit && hitCar.collider.gameObject == this.gameObject)
            carHit = false;

        // -- 检测路口 --
        RaycastHit hitInt;
        bool intersectionHit = Physics.SphereCast(origin, sensorRadius, transform.forward, out hitInt,
                                                  checkDistance, intersectionLayer, QueryTriggerInteraction.Collide);

        bool currentRedLight = false;
        if (intersectionHit && hitInt.collider != null)
        {
            var inter = hitInt.collider.GetComponentInParent<IntersectionController>();
            if (inter != null)
            {
                bool green = inter.IsGreenForDirection(transform.forward);
                if (!green) currentRedLight = true;
            }
        }

        // 更新前车与红灯状态
        carInFront = carHit;
        lightIsRed = currentRedLight;


        // ① 若检测到前车或红灯 → 立即进入停车锁状态
        if (carInFront || lightIsRed)
        {
            stopLocked = true;
        }
        else
        {
            // ② 若两者都消失 → 车辆解锁可继续前进
            stopLocked = false;
        }

        // ③ 把状态传给 WaypointFollower
        follower.shouldStop = stopLocked;

        // Debug 可视化
        DebugDraw(origin, checkDistance, carHit, hitCar, intersectionHit, hitInt);
    }

    void DebugDraw(Vector3 origin, float dist,
                   bool carHit, RaycastHit hitCar,
                   bool intHit, RaycastHit hitInt)
    {
        Debug.DrawLine(origin, origin + transform.forward * dist, Color.cyan);

        if (carHit)
            Debug.DrawLine(origin, hitCar.point, Color.red);

        if (intHit)
            Debug.DrawLine(origin, hitInt.point, Color.magenta);
    }
}

