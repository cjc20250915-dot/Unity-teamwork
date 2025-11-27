using UnityEngine;

[RequireComponent(typeof(WaypointFollower))]
public class CarSensor : MonoBehaviour
{
    public float frontCheckDistance = 3f;
    public LayerMask carLayer;
    public LayerMask intersectionLayer;
    WaypointFollower follower;

    void Awake()
    {
        follower = GetComponent<WaypointFollower>();
    }

    void FixedUpdate()
    {
        // 1) 前车检测 (射线)
        RaycastHit hit;
        bool carAhead = Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out hit, frontCheckDistance, carLayer);
        if (carAhead)
        {
            follower.shouldStop = true;
            return;
        }

        // 2) 红灯检测：射线检测前方如果是 IntersectionTrigger（trigger collider），再从它获取 IntersectionController
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out hit, frontCheckDistance, intersectionLayer))
        {
            var inter = hit.collider.GetComponentInParent<IntersectionController>();
            if (inter != null)
            {
                // Determine vehicle travel direction relative to intersection: use transform.forward
                bool green = inter.IsGreenForDirection(transform.forward);
                follower.shouldStop = !green;
                return;
            }
        }

        follower.shouldStop = false;
    }
}
