using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrafficGameController : MonoBehaviour
{
    public static TrafficGameController Instance;

    // Result Panels
    public GameObject goldUI;
    public GameObject silverUI;
    public GameObject copperUI;

    // Progress bar / Numeric UI
    public Text progressText;               
    public TextMeshProUGUI progressTMP;

    // Collision Ratio UI
    public Text crashRatioText;
    public TextMeshProUGUI crashRatioTMP;


    int totalPlannedCars = 0;   // Total number of departure gates
    int processedCars = 0;      // Cars that have already left the scene (either completed their journey normally or were crashed)
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

        // Find all generators
        spawners.AddRange(FindObjectsByType<CarSpawner>(FindObjectsSortMode.None));

        // Calculate the total number of departures
        foreach (var sp in spawners)
        {
            if (sp.maxTotalCars > 0)
                totalPlannedCars += sp.maxTotalCars;
        }

        Debug.Log($"TrafficGameController: plannedCars = {totalPlannedCars}");
    }

    // Vehicle crash
    public void OnCarCrash()
    {
        crashedCars++;
        processedCars++;   // Vehicles that have already been processed
    }

    // The vehicle has completed its journey normally (to the destination).
    public void OnCarFinished()
    {
        processedCars++;
    }

    // All spawners complete
    public void NotifySpawnerFinished()
    {
        foreach (var sp in spawners)
        {
            if (!sp.finished)
                return;
        }

        allSpawnersFinished = true;
        Debug.Log(">>> all spawners finished.");
    }

    void Update()
    {
        UpdateProgressUI();

        if (ended) return;
        if (!allSpawnersFinished) return;
        if (processedCars < totalPlannedCars) return;  // New logic: Wait until all cars are "processed"

        EndGame();
    }

    // Update the visual UI
    void UpdateProgressUI()
    {
        if (totalPlannedCars == 0) return;

        float ratio = (float)processedCars / totalPlannedCars;
        string txt = $"Progress: {processedCars}/{totalPlannedCars} ({ratio:P0})";

        if (progressText) progressText.text = txt;
        if (progressTMP) progressTMP.text = txt;

        // Crash ratio: crashedCars / total
        float crashRatio = (float)crashedCars / totalPlannedCars;
        string ctxt = $"{crashedCars}/{totalPlannedCars} ";

        if (crashRatioText) crashRatioText.text = ctxt;
        if (crashRatioTMP) crashRatioTMP.text = ctxt;
    }

    void EndGame()
    {
        ended = true;
        Time.timeScale = 0f;

        float crashRatio = totalPlannedCars == 0 ? 1 : (float)crashedCars / totalPlannedCars;

        if (crashRatio == 0f) goldUI?.SetActive(true);
        else if (crashRatio <= 0.3) silverUI?.SetActive(true);
        else copperUI?.SetActive(true);

        Debug.Log($"Game Finished. processed={processedCars}, total={totalPlannedCars}");
    }
}
