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

    // Added: Parking lock
    bool stopLocked = false;       // Lock the car when entering the inspection area
    bool lightIsRed = false;       //Red light status
    bool carInFront = false;       // Front vehicle status

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

        // Detect the preceding vehicle (SphereCast)
        RaycastHit hitCar;
        bool carHit = Physics.SphereCast(origin, sensorRadius, transform.forward, out hitCar,
                                         checkDistance, carLayer, QueryTriggerInteraction.Ignore);

        // Clean up: Ignore yourself
        if (carHit && hitCar.collider.gameObject == this.gameObject)
            carHit = false;

        // Detection intersection
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

        // Update the status of the vehicle in front and the red light
        carInFront = carHit;
        lightIsRed = currentRedLight;


        // If a vehicle ahead or a red light is detected, immediately engage the parking lock.
        if (carInFront || lightIsRed)
        {
            stopLocked = true;
        }
        else
        {
            // If both disappear, the vehicle can be unlocked and you can continue.
            stopLocked = false;
        }

        // Pass the state to WaypointFollower
        follower.shouldStop = stopLocked;

        // Debug
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

