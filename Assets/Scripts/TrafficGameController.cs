using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrafficGameController : MonoBehaviour
{
    public static TrafficGameController Instance;

    [Header("Result Panels")]
    public GameObject goldUI;
    public GameObject silverUI;
    public GameObject copperUI;

    [Header("进度条 / 数字 UI")]
    public Text progressText;               // 普通 Text
    public TextMeshProUGUI progressTMP;     // TMP 可选

    [Header("撞车比例 UI")]
    public Text crashRatioText;
    public TextMeshProUGUI crashRatioTMP;


    int totalPlannedCars = 0;   // 所有发车口的总量
    int processedCars = 0;      // 已经离场的车（正常走完或撞毁）
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

        // 计算所有发车数量
        foreach (var sp in spawners)
        {
            if (sp.maxTotalCars > 0)
                totalPlannedCars += sp.maxTotalCars;
        }

        Debug.Log($"TrafficGameController: plannedCars = {totalPlannedCars}");
    }

    // 车辆撞毁
    public void OnCarCrash()
    {
        crashedCars++;
        processedCars++;   // 属于已处理车辆
    }

    // 车辆正常结束（到终点）
    public void OnCarFinished()
    {
        processedCars++;
    }

    // 所有 spawner 完成
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
        if (processedCars < totalPlannedCars) return;  // 新逻辑：等所有车都“处理完”

        EndGame();
    }

    // 更新可视化 UI
    void UpdateProgressUI()
    {
        if (totalPlannedCars == 0) return;

        float ratio = (float)processedCars / totalPlannedCars;
        string txt = $"Progress: {processedCars}/{totalPlannedCars} ({ratio:P0})";

        if (progressText) progressText.text = txt;
        if (progressTMP) progressTMP.text = txt;

        //  撞车比值：crashedCars / total 
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
        else if (crashRatio >= 0.7) silverUI?.SetActive(true);
        else copperUI?.SetActive(true);

        Debug.Log($"Game Finished. processed={processedCars}, total={totalPlannedCars}");
    }
}
