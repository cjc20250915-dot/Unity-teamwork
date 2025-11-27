using System;
using UnityEngine;

public class IntersectionController : MonoBehaviour
{
    public enum IntersectionState { NS_Green, EW_Green }
    public IntersectionState currentState = IntersectionState.NS_Green;
    public float greenDuration = 8f;
    public float yellowDuration = 2f;

    private float timer = 0f;
    private bool isYellow = false;

    public Action<IntersectionState> OnStateChanged; // optional listeners

    void Start()
    {
        timer = greenDuration;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            if (!isYellow)
            {
                // go yellow
                isYellow = true;
                timer = yellowDuration;
                // you might want to notify lights to show yellow (if implemented)
            }
            else
            {
                // switch green direction
                isYellow = false;
                currentState = currentState == IntersectionState.NS_Green ? IntersectionState.EW_Green : IntersectionState.NS_Green;
                timer = greenDuration;
                OnStateChanged?.Invoke(currentState);
            }
        }
    }

    // Called by player to toggle immediately (click)
    public void ToggleImmediate()
    {
        isYellow = false;
        currentState = currentState == IntersectionState.NS_Green ? IntersectionState.EW_Green : IntersectionState.NS_Green;
        timer = greenDuration;
        OnStateChanged?.Invoke(currentState);
    }

    public bool IsGreenForDirection(Vector3 fromDirection)
    {
        // Simplified: assume world Z axis is North-South. Use dot to decide.
        float dot = Vector3.Dot(fromDirection.normalized, Vector3.forward); // >0 => northward
        bool isNS = Mathf.Abs(dot) > 0.5f; // roughly N/S
        if (isNS) return currentState == IntersectionState.NS_Green;
        return currentState == IntersectionState.EW_Green;
    }
}
