using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    
    public List<GameObject> carPrefabs;    // Added: Random generation of multiple car models


    public List<Transform> pathWaypoints;  // Waypoints

    // Generation interval
    public float spawnIntervalMin = 1.2f;
    public float spawnIntervalMax = 3.0f;

    // Maximum number of vehicles that can exist at the same time
    public int maxConcurrent = 6;

    // Total number of generators capped (-1 = unlimit)
    public int maxTotalCars = -1;

    [HideInInspector] public bool finished = false; // Whether the generation is completed, though actually I feel this isn't very useful


    int aliveCount = 0;
    int totalSpawned = 0;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        // Infinite loop — exits only when the total amount generated is reached.
        while (true)
        {
            // Reaching the total generation limit， Exiting the loop 
            if (maxTotalCars >= 0 && totalSpawned >= maxTotalCars)
                break;

            // Current survival limit not reached ，only generated now
            if (aliveCount < maxConcurrent)
            {
                SpawnCar();
            }

            float wait = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(wait);
        }

        // Move out of the while loop to ensure it always executes to this point.
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

        // Randomly select one from 6 prefabs

        GameObject prefab = carPrefabs[Random.Range(0, carPrefabs.Count)];

        GameObject go = Instantiate(prefab, transform.position, transform.rotation);

        aliveCount++;
        totalSpawned++;

        // Set path and speed
        var wf = go.GetComponent<WaypointFollower>();
        if (wf != null)
        {
            wf.waypoints = new List<Transform>(pathWaypoints);
            wf.maxSpeed = Random.Range(4.0f, 8.0f);
        }

        // Decrease aliveCount after the vehicle is destroyed
        StartCoroutine(WaitUntilDestroyed(go));
    }

    IEnumerator WaitUntilDestroyed(GameObject go)
    {
        while (go != null && go)
            yield return null;

        aliveCount = Mathf.Max(0, aliveCount - 1);
    }
}
