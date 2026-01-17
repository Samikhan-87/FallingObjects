using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameDifficulty currentDifficulty;
    public float timeRemaining;
    public int score;
    public bool useTimer = true;

    // UI References
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public GameObject timerIcon; // ✅ Timer icon button

    // Game Over UI
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    // Pause UI
    public GameObject pausePanel;

    // Settings UI
    public GameObject settingsPanel;

    private bool isGameOver = false;
    private bool isPaused = false;
    private bool isSettingsOpen = false;
    private GameObject previousPanel;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;
        currentDifficulty = (GameDifficulty)PlayerPrefs.GetInt("Difficulty", 0);

        // Initialize UI
        UpdateScoreUI();
        UpdateTimerUI();

        // Hide all panels at start
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // Start coroutine to apply difficulty
        StartCoroutine(InitializeGame());
    }

    IEnumerator InitializeGame()
    {
        yield return new WaitForEndOfFrame();
        ApplyDifficulty();
    }

    void Update()
    {
        // SPACE key to toggle pause
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && !isGameOver && !isSettingsOpen)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        if (isGameOver || isPaused) return;

        if (!useTimer) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            GameOver();
        }

        UpdateTimerUI();
    }

    void ApplyDifficulty()
    {
        switch (currentDifficulty)
        {
            case GameDifficulty.Easy:
                timeRemaining = 60f;
                useTimer = true;
                // ✅ Show timer UI
                ShowTimerUI(true);
                if (BladeSpawner.instance != null)
                    BladeSpawner.instance.DisableBlades();
                break;

            case GameDifficulty.Medium:
                timeRemaining = 60f;
                useTimer = true;
                // ✅ Show timer UI
                ShowTimerUI(true);
                if (BladeSpawner.instance != null)
                    BladeSpawner.instance.EnableBlades();
                break;

            case GameDifficulty.Hard:
                timeRemaining = 0f;
                useTimer = false;
                // ✅ Hide timer UI on Hard
                ShowTimerUI(false);
                if (BladeSpawner.instance != null)
                    BladeSpawner.instance.EnableBlades();
                break;
        }
    }

    // ✅ NEW: Show/Hide Timer UI
    void ShowTimerUI(bool show)
    {
        if (timerText != null)
            timerText.gameObject.SetActive(show);

        if (timerIcon != null)
            timerIcon.SetActive(show);
    }

    public void ReduceTime(float amount)
    {
        if (isGameOver || !useTimer) return;

        timeRemaining -= amount;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            GameOver();
        }

        UpdateTimerUI();
    }

    // ✅ NEW: Bomb hit on Hard difficulty = instant death
    public void BombHit()
    {
        if (isGameOver) return;

        if (currentDifficulty == GameDifficulty.Hard)
        {
            // ✅ Hard mode: Bomb = Game Over
            InstantDeath();
        }
        else
        {
            // Easy/Medium: Reduce time
            ReduceTime(5f);
        }
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;
        UpdateScoreUI();
    }

    public void InstantDeath()
    {
        GameOver();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "SCORE: " + score.ToString("D4");
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            if (useTimer)
                timerText.text = "TIMER: " + Mathf.CeilToInt(timeRemaining);
            else
                timerText.text = "";
        }
    }

    void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (finalScoreText != null)
            {
                finalScoreText.text = score.ToString("D4");
                finalScoreText.alignment = TextAlignmentOptions.Center;
            }
        }

        Debug.Log("GAME OVER - Final Score: " + score);
    }

    // ========== PAUSE MENU FUNCTIONS ==========

    public void PauseGame()
    {
        if (isGameOver) return;

        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Debug.Log("Game Resumed");
    }

    public void CancelPause()
    {
        ResumeGame();
    }

    // ========== SETTINGS PANEL FUNCTIONS ==========

    public void OpenSettingsFromPause()
    {
        previousPanel = pausePanel;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        isSettingsOpen = true;
        Debug.Log("Settings opened from Pause");
    }

    public void OpenSettingsFromGameOver()
    {
        previousPanel = gameOverPanel;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        isSettingsOpen = true;
        Debug.Log("Settings opened from GameOver");
    }

    public void CloseSettings()
    {
        isSettingsOpen = false;

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (previousPanel != null)
            previousPanel.SetActive(true);

        Debug.Log("Settings closed");
    }

    // ========== AUDIO FUNCTIONS ==========

    public void ToggleMusicFromSettings()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.ToggleMusic();
    }

    public void ToggleSoundFromSettings()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.ToggleSound();
    }

    // ========== BUTTON FUNCTIONS ==========

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void PauseRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToLevels()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelSelection");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
        Debug.Log("Quit to Main Menu");
    }
}