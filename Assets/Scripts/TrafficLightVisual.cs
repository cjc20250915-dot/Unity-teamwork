using UnityEngine;

public class TrafficLightVisual : MonoBehaviour
{
    public GameObject greenLight;
    public GameObject redLight;
    public IntersectionController controller;

    [Header("此灯是否是南北方向的灯？")]
    public bool isNorthSouth = true;

    void Start()
    {
        if (controller != null)
            controller.OnStateChanged += UpdateVisual;

        // 初始化显示
        UpdateVisual(controller.currentState);
    }

    void UpdateVisual(IntersectionController.IntersectionState state)
    {
        // 是否是当前方向的绿灯
        bool nsGreen = (state == IntersectionController.IntersectionState.NS_Green);

        // 若本灯属于 NS 方向，则绿灯取 nsGreen；若属于 EW，则取 !nsGreen
        bool myGreen = isNorthSouth ? nsGreen : !nsGreen;

        greenLight.SetActive(myGreen);
        redLight.SetActive(!myGreen);
    }
}
