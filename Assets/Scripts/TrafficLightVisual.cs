using UnityEngine;

public class TrafficLightVisual : MonoBehaviour
{
    public GameObject greenLight;
    public GameObject redLight;
    public IntersectionController controller;

    // Is this light for the north-south direction?
    public bool isNorthSouth = true;

    void Start()
    {
        if (controller != null)
            controller.OnStateChanged += UpdateVisual;

        // Initialize display
        UpdateVisual(controller.currentState);
    }

    void UpdateVisual(IntersectionController.IntersectionState state)
    {
        // Is it a green light for the current direction?
        bool nsGreen = (state == IntersectionController.IntersectionState.NS_Green);

        // If this light is in the NS direction, the green light value is nsGreen; if it is in the EW direction, the green light value is !nsGreen.
        bool myGreen = isNorthSouth ? nsGreen : !nsGreen;

        greenLight.SetActive(myGreen);
        redLight.SetActive(!myGreen);
    }
}
