using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("Main properties")]
    [SerializeField]
    private GameObject pressAnyKeyText;
    [SerializeField]
    private Text timerNumberText;
    [SerializeField]
    private Text currentScoreNumberText;
    [SerializeField]
    private Text highScoreNumberText;
    [SerializeField]
    private Text asteroidsNumberText;
    [SerializeField]
    private GameObject highScorePanel;
    [SerializeField]
    private Text newHighScoreNumberText;

    [Header("Game Over panel properties")]
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private Text gameOverScoreNumberText;
    [SerializeField]
    private Text gameOverTimeNumberText;
    [SerializeField]
    private Text gameOverAsteroidsNumberText;

    public static GameUI instance;

    private void Awake()
    {
        instance = this;

        // Events binding
        GameEvents.instance.onStartGame += RemoveStartText;
    }

    // Remove press any key text
    private void RemoveStartText()
    {
        pressAnyKeyText.SetActive(false);
    }

    // Time display function (minutes:seconds) 
    public void DisplayTime(float timeValue)
    {
        float minutes = Mathf.FloorToInt(timeValue / 60);
        float seconds = Mathf.FloorToInt(timeValue % 60);

        timerNumberText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Score display function
    public void DisplayScore(int score)
    {
        currentScoreNumberText.text = score.ToString();
    }

    // High score display function
    public void DisplayHighScore(int highScore)
    {
        highScoreNumberText.text = highScore.ToString();
    }

    // Passed asteroids display function
    public void DisplayAsteroids(int amount)
    {
        asteroidsNumberText.text = amount.ToString();
    }

    // Game Over panel set up and display
    public void DisplayGameOver(int score, float time, int asteroids, bool isNewHighScore)
    {
        gameOverScoreNumberText.text = score.ToString();
        gameOverAsteroidsNumberText.text = asteroids.ToString();

        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        gameOverTimeNumberText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Wait before displaying pop ups
        StartCoroutine(WaitGameOver(1f, isNewHighScore, score));
    }

    private IEnumerator WaitGameOver(float seconds, bool isNewHighScore, int score)
    {
        yield return new WaitForSeconds(seconds);
        gameOverPanel.SetActive(true);

        // If broke high score show new high score pop up
        if (isNewHighScore)
        {
            newHighScoreNumberText.text = score.ToString();
            highScorePanel.SetActive(true);
        }
    }

    // Restart button click function
    public void onButtonRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // New high score OK click function
    public void onButtonHighScoreOk()
    {
        highScorePanel.SetActive(false);
    }
}