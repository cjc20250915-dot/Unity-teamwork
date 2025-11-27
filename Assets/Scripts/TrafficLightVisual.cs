using UnityEngine;

public class TrafficLightVisual : MonoBehaviour
{
    public GameObject redLight;
    public GameObject yellowLight;
    public GameObject greenLight;
    public IntersectionController controller;
    void Start()
    {
        if (controller != null) controller.OnStateChanged += OnStateChanged;
        UpdateVisual();
    }
    void OnDestroy()
    {
        if (controller != null) controller.OnStateChanged -= OnStateChanged;
    }
    void OnStateChanged(IntersectionController.IntersectionState state)
    {
        UpdateVisual();
    }
    void Update()
    {
        // if controller has yellow internal flag, we could show yellow; omitted for brevity
    }
    void UpdateVisual()
    {
        if (controller == null) return;
        var st = controller.currentState;
        redLight.SetActive(st != IntersectionController.IntersectionState.NS_Green && st != IntersectionController.IntersectionState.EW_Green); // fallback
        // crude: show green for NS or EW accordingly
        // We'll just alternate: when NS green -> activate corresponding light object(s).
        // For simplicity here, show greenLight active always when st == NS_Green (tweak to show per direction in refined setups)
        greenLight.SetActive(true);
        redLight.SetActive(false);
        yellowLight.SetActive(false);
    }
}
