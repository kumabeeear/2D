using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Panels")]
    public GameObject gamePanel;
    public GameObject blackPanel;

    [Header("Spawners")]
    public ChopstickSpawner chopstickSpawner;
    public BirdSpawner birdSpawner;
    public PoopSpawner poopSpawner;
    public CoinSpawner coinSpawner;

    [Header("Level Settings")]
    public float levelDuration = 120f;

    private bool isGameRunning = false;
    private Coroutine levelTimerCoroutine;

    void Awake()
    {
        Instance = this;

        if (blackPanel != null)
            blackPanel.SetActive(false);
    }

    public void StartLevel(int levelNumber)
    {
        isGameRunning = true;

        if (blackPanel != null)
            blackPanel.SetActive(false);

        if (gamePanel != null)
            gamePanel.SetActive(true);

        if (chopstickSpawner != null)
            chopstickSpawner.StartSpawning();

        if (birdSpawner != null)
        {
            if (levelNumber >= 2)
                birdSpawner.StartSpawning();
            else
                birdSpawner.StopSpawning();
        }

        if (poopSpawner != null)
        {
            if (levelNumber >= 3)
                poopSpawner.StartSpawning();
            else
                poopSpawner.StopSpawning();
        }

        if (coinSpawner != null)
            coinSpawner.StartSpawning();

        Time.timeScale = 1f;

        if (levelTimerCoroutine != null)
            StopCoroutine(levelTimerCoroutine);

        levelTimerCoroutine = StartCoroutine(LevelTimer(levelNumber));
    }

    IEnumerator LevelTimer(int levelNumber)
    {
        yield return new WaitForSeconds(levelDuration);

        if (!isGameRunning) yield break;

        isGameRunning = false;
        StopAllSpawners();

        if (levelNumber == 1)
            FlowManager.Instance.Level1Complete();
        else if (levelNumber == 2)
            FlowManager.Instance.Level2Complete();
        else if (levelNumber == 3)
        {
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.ShowFinalScore();

            FlowManager.Instance.Level3Complete();
        }
    }

    public void GameOver()
    {
        if (!isGameRunning) return;

        isGameRunning = false;
        StopAllSpawners();

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ShowFinalScore();

        Time.timeScale = 0f;

        if (blackPanel != null)
            blackPanel.SetActive(true);
    }

    void StopAllSpawners()
    {
        if (chopstickSpawner != null)
            chopstickSpawner.StopSpawning();

        if (birdSpawner != null)
            birdSpawner.StopSpawning();

        if (poopSpawner != null)
            poopSpawner.StopSpawning();

        if (coinSpawner != null)
            coinSpawner.StopSpawning();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}