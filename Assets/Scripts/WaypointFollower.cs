using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WaypointFollower : MonoBehaviour
{
    public List<Transform> waypoints;
    public float maxSpeed = 6f;
    public float acceleration = 8f;
    public float stoppingDistance = 0.5f;

    // external control
    [HideInInspector] public bool shouldStop = false; // e.g., red light or car ahead
    Rigidbody rb;
    int wpIndex = 0;
    float currentSpeed = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        if (waypoints == null || waypoints.Count == 0) return;
        if (wpIndex >= waypoints.Count) { Destroy(gameObject, 2f); return; } // reached end

        Vector3 target = waypoints[wpIndex].position;
        Vector3 dir = (target - transform.position);
        float distance = dir.magnitude;

        // speed control
        float targetSpeed = shouldStop ? 0f : maxSpeed;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);

        // movement
        if (distance > stoppingDistance)
        {
            Vector3 move = dir.normalized * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
            // face moving direction smoothly
            if (move.sqrMagnitude > 0.0001f)
            {
                Quaternion toRot = Quaternion.LookRotation(move.normalized, Vector3.up);
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, toRot, 10f * Time.fixedDeltaTime));
            }
        }
        else
        {
            wpIndex++;
        }
    }
}
