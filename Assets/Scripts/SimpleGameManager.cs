using UnityEngine;
using UnityEngine.UI;

public class SimpleGameManager : MonoBehaviour
{
    public static SimpleGameManager Instance;
    public Text scoreText;
    public GameObject gameOverPanel;
    public int collisionsToLose = 1;
    int collisions = 0;
    float score = 0f;
    bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Update()
    {
        if (isGameOver) return;
        score += Time.deltaTime;
        if (scoreText) scoreText.text = $"Score: {Mathf.FloorToInt(score)}";
    }

    public void ReportCollision()
    {
        collisions++;
        if (collisions >= collisionsToLose) GameOver();
    }

    public void GameOver()
    {
        isGameOver = true;
        if (gameOverPanel) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
