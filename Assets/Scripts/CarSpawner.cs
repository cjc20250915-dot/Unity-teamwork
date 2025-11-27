using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject carPrefab;
    public List<Transform> pathWaypoints; // assign the waypoint sequence for this spawn
    public float spawnIntervalMin = 1.2f;
    public float spawnIntervalMax = 3.0f;
    public int maxConcurrent = 6;

    int aliveCount = 0;
    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (aliveCount < maxConcurrent)
            {
                SpawnCar();
            }
            float wait = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(wait);
        }
    }

    void SpawnCar()
    {
        GameObject go = Instantiate(carPrefab, transform.position, transform.rotation);
        aliveCount++;
        var wf = go.GetComponent<WaypointFollower>();
        if (wf != null)
        {
            wf.waypoints = new List<Transform>(pathWaypoints);
            wf.maxSpeed = Random.Range(4.0f, 8.0f);
        }
        var coll = go.GetComponent<Collider>();
        var rb = go.GetComponent<Rigidbody>();
        // subscribe to destroy hook to decrement aliveCount
        StartCoroutine(WaitUntilDestroyed(go));
    }

    System.Collections.IEnumerator WaitUntilDestroyed(GameObject go)
    {
        while (go != null) yield return null;
        aliveCount = Mathf.Max(0, aliveCount - 1);
    }
}
