using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Score UI")]
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;

    private int score = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ResetScore();
        HideScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();

        if (finalScoreText != null)
            finalScoreText.text = "";
    }

    public void ShowFinalScore()
    {
        if (finalScoreText != null)
            finalScoreText.text = "Score: " + score;
    }

    public void ShowScoreUI()
    {
        if (scoreText != null)
            scoreText.gameObject.SetActive(true);
    }

    public void HideScoreUI()
    {
        if (scoreText != null)
            scoreText.gameObject.SetActive(false);
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}