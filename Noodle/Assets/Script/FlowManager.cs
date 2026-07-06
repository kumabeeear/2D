using UnityEngine;
using UnityEngine.UI;

public class FlowManager : MonoBehaviour
{
    public static FlowManager Instance;

    [Header("Panels")]
    public GameObject startPanel;
    public GameObject comicPanel;
    public GameObject gamePanel;
    public GameObject endPanel;

    [Header("Comic UI")]
    public Image comicImage;

    [Header("Comics")]
    public Sprite[] openingComics;
    public Sprite betweenLevel1And2Comic;
    public Sprite betweenLevel2And3Comic;
    public Sprite endingComic;

    private int comicIndex = 0;
    private string currentComicType = "";

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ShowStart();
    }

    public void ShowStart()
    {
        startPanel.SetActive(true);
        comicPanel.SetActive(false);
        gamePanel.SetActive(false);

        if (endPanel != null)
            endPanel.SetActive(false);

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.HideScoreUI();
    }

    public void StartGameFlow()
    {
        startPanel.SetActive(false);
        ShowOpeningComic();
    }

    public void ShowOpeningComic()
    {
        currentComicType = "Opening";
        comicIndex = 0;

        comicPanel.SetActive(true);
        gamePanel.SetActive(false);

        if (endPanel != null)
            endPanel.SetActive(false);

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.HideScoreUI();

        if (openingComics.Length > 0)
            comicImage.sprite = openingComics[comicIndex];
    }

    public void NextComic()
    {
        if (currentComicType == "Opening")
        {
            comicIndex++;

            if (comicIndex >= openingComics.Length)
            {
                StartLevel1();
            }
            else
            {
                comicImage.sprite = openingComics[comicIndex];
            }
        }
        else if (currentComicType == "Between1And2")
        {
            StartLevel2();
        }
        else if (currentComicType == "Between2And3")
        {
            StartLevel3();
        }
        else if (currentComicType == "Ending")
        {
            ShowEndPanel();
        }
    }

    public void StartLevel1()
    {
        comicPanel.SetActive(false);

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ShowScoreUI();

        GameManager.Instance.StartLevel(1);
    }

    public void StartLevel2()
    {
        comicPanel.SetActive(false);

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ShowScoreUI();

        GameManager.Instance.StartLevel(2);
    }

    public void StartLevel3()
    {
        comicPanel.SetActive(false);

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ShowScoreUI();

        GameManager.Instance.StartLevel(3);
    }

    public void Level1Complete()
    {
        gamePanel.SetActive(false);
        comicPanel.SetActive(true);

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.HideScoreUI();

        currentComicType = "Between1And2";
        comicImage.sprite = betweenLevel1And2Comic;
    }

    public void Level2Complete()
    {
        gamePanel.SetActive(false);
        comicPanel.SetActive(true);

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.HideScoreUI();

        currentComicType = "Between2And3";
        comicImage.sprite = betweenLevel2And3Comic;
    }

    public void Level3Complete()
    {
        gamePanel.SetActive(false);
        comicPanel.SetActive(true);

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.HideScoreUI();

        currentComicType = "Ending";
        comicImage.sprite = endingComic;
    }

    public void ShowEndPanel()
    {
        comicPanel.SetActive(false);

        if (endPanel != null)
            endPanel.SetActive(true);
    }
}