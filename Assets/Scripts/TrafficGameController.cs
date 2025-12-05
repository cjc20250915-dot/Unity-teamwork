using System.Collections.Generic;
using UnityEngine;

public class TrafficGameController : MonoBehaviour
{
    public static TrafficGameController Instance;

    [Header("Result Panels")]
    public GameObject goldUI;
    public GameObject silverUI;
    public GameObject copperUI;

    int totalPlannedCars = 0;
    int crashedCars = 0;

    List<CarSpawner> spawners = new List<CarSpawner>();
    bool allSpawnersFinished = false;
    bool ended = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (goldUI) goldUI.SetActive(false);
        if (silverUI) silverUI.SetActive(false);
        if (copperUI) copperUI.SetActive(false);

        // 找到所有生成器
        spawners.AddRange(FindObjectsByType<CarSpawner>(FindObjectsSortMode.None));

        // 计算所有发车口的总数
        foreach (var sp in spawners)
        {
            if (sp.maxTotalCars > 0)
                totalPlannedCars += sp.maxTotalCars;
        }

        Debug.Log($"TrafficGameController: plannedCars = {totalPlannedCars}");
    }

    // 用于 spawner 在生成车辆时调用
    public void OnCarSpawn() { }

    // 撞车时调用
    public void OnCarCrash()
    {
        crashedCars++;
    }

    // 由 CarSpawner 在发完所有车时调用
    public void NotifySpawnerFinished()
    {
        // 当所有 Spawner 都调用过此函数时才真正 finished
        foreach (var sp in spawners)
        {
            if (!sp.finished)
                return; // 有未完成的
        }

        allSpawnersFinished = true;
        Debug.Log(">>> all spawners finished.");
    }

    void Update()
    {
        if (ended) return;
        if (!allSpawnersFinished) return;

        // ----------- 关键逻辑：检查是否场景中还有车辆 -------------
        var cars = GameObject.FindGameObjectsWithTag("Car");
        if (cars.Length == 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        ended = true;
        Time.timeScale = 0f;

        float ratio = totalPlannedCars == 0 ? 1 : (float)crashedCars / totalPlannedCars;

        if (ratio == 0f) goldUI?.SetActive(true);
        else if (ratio >= 0.7) silverUI?.SetActive(true);
        else copperUI?.SetActive(true);

        Debug.Log($"Game Finished. Crashed={crashedCars}, Planned={totalPlannedCars}, Ratio={ratio}");
    }
}
