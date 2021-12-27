using System.Collections;
using UnityEngine;

public class SessionStats : MonoBehaviour
{
    public static SessionStats instance;

    [Header("Score properties")]
    private float scoreCountingIntervalSeconds = 1f;
    [SerializeField]
    private float speedUpScoreCountingIntervalSeconds = 0.5f;
    [SerializeField]
    private int constantScoreIncreaseAmount = 1;
    [SerializeField]
    private int asteroidPassScoreIncreaseAmount = 5;

    private float timeValue = 0f;
    private float scoreIncreaseRate;

    private int currentScore = 0;
    private int currentAsteroids = 0;

    private bool isNewHighScore = false;
    private bool isGameStarted = false;
    private bool isGameOver = false;

    private void Awake()
    {
        instance = this;

        // Events binding
        GameEvents.instance.onStartGame += StartGame;
        GameEvents.instance.onSpeedUp += SetScoreWaitSpeedUp;
        GameEvents.instance.onSpeedDown += SetScoreWaitSpeedDown;
        GameEvents.instance.onAsteroidPassed += AsteroidPass;
        GameEvents.instance.onGameOver += GameOverSetUp;
    }

    void Start()
    {
        //PlayerPrefs.SetInt("HighScore", 0);
        // Set high score
        GameUI.instance.DisplayHighScore(PlayerPrefs.GetInt("HighScore", 0));
        // Set default score waiting time
        scoreIncreaseRate = scoreCountingIntervalSeconds;
    }

    private void StartGame()
    {
        isGameStarted = true;
        // Start score counting
        StartCoroutine(ScoreCounting());
    }

    private void Update()
    {
        if (!isGameOver && isGameStarted)
        {
            timeValue += Time.deltaTime;
            GameUI.instance.DisplayTime(timeValue);
        }
    }

    // Add currentScore every scoreWaitSeconds seconds
    // and display on screen
    private IEnumerator ScoreCounting()
    {
        while (!isGameOver)
        {
            AddScore(constantScoreIncreaseAmount);

            yield return new WaitForSeconds(scoreIncreaseRate);
        }
    }

    // Add amount of score to currentScore
    private void AddScore(int amount)
    {
        currentScore += amount;
        GameUI.instance.DisplayScore(currentScore);

        UpdateHighScore();
    }

    // Update high score if needed
    private void UpdateHighScore()
    {
        if (PlayerPrefs.GetInt("HighScore", 0) < currentScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            GameUI.instance.DisplayHighScore(currentScore);
            if (!isNewHighScore)
                isNewHighScore = true;
        }
    }

    // Inrease score counting rate if
    // speed up event triggered
    private void SetScoreWaitSpeedUp()
    {
        scoreIncreaseRate = speedUpScoreCountingIntervalSeconds;
    }

    // Decrease score counting rate if
    // speed down event triggered
    private void SetScoreWaitSpeedDown()
    {
        scoreIncreaseRate = scoreCountingIntervalSeconds;
    }

    // Increase score and asteroids count if asteroid passed
    private void AsteroidPass()
    {
        AddScore(asteroidPassScoreIncreaseAmount);

        GameUI.instance.DisplayAsteroids(++currentAsteroids);
    }

    private void GameOverSetUp()
    {
        isGameOver = true;
        GameUI.instance.DisplayGameOver(currentScore, timeValue, currentAsteroids, isNewHighScore);
    }
}