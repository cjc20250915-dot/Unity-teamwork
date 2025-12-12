using UnityEngine;

public class IntersectionController : MonoBehaviour
{
    public enum IntersectionState
    {
        NS_Green,
        EW_Green
    }

    public IntersectionState currentState = IntersectionState.NS_Green;

    public float greenDuration = 5f;
    public float yellowDuration = 2f;

    public bool isYellow = false;

    public delegate void StateChanged(IntersectionState newState);
    public event StateChanged OnStateChanged;

    float timer = 0f;

    void Start()
    {
        timer = greenDuration;

        // Broadcast once at startup to allow the lights to initialize properly.
        OnStateChanged?.Invoke(currentState);
    }

    void Update()
    {
        HandleManualInput();
    }

    void HandleManualInput()
    {
        // Press S ,North-South green light
        if (Input.GetKeyDown(KeyCode.S))
        {
            currentState = IntersectionState.NS_Green;
            isYellow = false;
            OnStateChanged?.Invoke(currentState);
        }

        // Press D,East-West green light
        if (Input.GetKeyDown(KeyCode.D))
        {
            currentState = IntersectionState.EW_Green;
            isYellow = false;
            OnStateChanged?.Invoke(currentState);
        }
    }

    // Green light query interface provided to CarSensor
    public bool IsGreenForDirection(Vector3 dir)
    {
        bool northSouth = Mathf.Abs(dir.z) > Mathf.Abs(dir.x);

        if (northSouth)
            return currentState == IntersectionState.NS_Green;
        else
            return currentState == IntersectionState.EW_Green;
    }
}
