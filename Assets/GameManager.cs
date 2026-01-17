using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public float timeRemaining = 60f;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    bool isGameOver = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreUI();
        UpdateTimerUI();
    }

    void Update()
    {
        if (isGameOver) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            GameOver();
        }

        UpdateTimerUI();
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;
        UpdateScoreUI();
    }

    public void ReduceTime(float amount)
    {
        if (isGameOver) return;

        timeRemaining -= amount;

        if (timeRemaining < 0)
            timeRemaining = 0;

        UpdateTimerUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }

    void UpdateTimerUI()
    {
        timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining);
    }

    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("GAME OVER");
    }
}
