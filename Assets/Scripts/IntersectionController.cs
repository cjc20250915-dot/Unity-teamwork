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

        // 启动时广播一次，让灯光正常初始化
        OnStateChanged?.Invoke(currentState);
    }

    void Update()
    {

        /*
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            if (!isYellow)
            {
                isYellow = true;
                timer = yellowDuration;
            }
            else
            {
                isYellow = false;
                currentState = currentState == IntersectionState.NS_Green ?
                               IntersectionState.EW_Green :
                               IntersectionState.NS_Green;

                timer = greenDuration;
                OnStateChanged?.Invoke(currentState);
            }
        }
        */

      
        HandleManualInput();
    }

    void HandleManualInput()
    {
        // 按 S → 南北方向绿灯
        if (Input.GetKeyDown(KeyCode.S))
        {
            currentState = IntersectionState.NS_Green;
            isYellow = false;
            OnStateChanged?.Invoke(currentState);
        }

        // 按 D → 东西方向绿灯
        if (Input.GetKeyDown(KeyCode.D))
        {
            currentState = IntersectionState.EW_Green;
            isYellow = false;
            OnStateChanged?.Invoke(currentState);
        }
    }

    // 提供给 CarSensor 的绿灯查询接口（未做修改）
    public bool IsGreenForDirection(Vector3 dir)
    {
        bool northSouth = Mathf.Abs(dir.z) > Mathf.Abs(dir.x);

        if (northSouth)
            return currentState == IntersectionState.NS_Green;
        else
            return currentState == IntersectionState.EW_Green;
    }
}
