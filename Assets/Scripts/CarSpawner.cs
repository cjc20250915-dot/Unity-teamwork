using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("车模型（在这里放 6 个 Variant）")]
    public List<GameObject> carPrefabs;    // 新增：多车型随机生成

    [Header("路径 Waypoints")]
    public List<Transform> pathWaypoints;

    [Header("生成间隔")]
    public float spawnIntervalMin = 1.2f;
    public float spawnIntervalMax = 3.0f;

    [Header("最大同时存在车辆数")]
    public int maxConcurrent = 6;

    [Header("总生成数量上限 (-1 = 不限制)")]
    public int maxTotalCars = -1;

    [HideInInspector] public bool finished = false;//是否完成生成的布尔值，其实感觉这个没啥用。


    int aliveCount = 0;
    int totalSpawned = 0;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        // 无限循环 —— 只有生成到总量才退出
        while (true)
        {
            // 达到总生成上限 → 跳出循环（不是 yield break）
            if (maxTotalCars >= 0 && totalSpawned >= maxTotalCars)
                break;

            // 当前存活未达上限 → 才生成
            if (aliveCount < maxConcurrent)
            {
                SpawnCar();
            }

            float wait = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(wait);
        }

        // 移出 while，让它一定能执行到这里
        finished = true;
        TrafficGameController.Instance?.NotifySpawnerFinished();
    }

    void SpawnCar()
    {
        if (carPrefabs == null || carPrefabs.Count == 0)
        {
            Debug.LogWarning("CarSpawner: 车预制体列表为空！");
            return;
        }

        // 从 6 个预制体中随机选择一个

        GameObject prefab = carPrefabs[Random.Range(0, carPrefabs.Count)];

        GameObject go = Instantiate(prefab, transform.position, transform.rotation);

        TrafficGameController.Instance.OnCarSpawn();

        aliveCount++;
        totalSpawned++;

        // 设置路径与速度
        var wf = go.GetComponent<WaypointFollower>();
        if (wf != null)
        {
            wf.waypoints = new List<Transform>(pathWaypoints);
            wf.maxSpeed = Random.Range(4.0f, 8.0f);
        }

        // 等待车辆销毁后减少 aliveCount
        StartCoroutine(WaitUntilDestroyed(go));
    }

    IEnumerator WaitUntilDestroyed(GameObject go)
    {
        while (go != null && go)
            yield return null;

        aliveCount = Mathf.Max(0, aliveCount - 1);
    }
}
