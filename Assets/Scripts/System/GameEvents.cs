using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    private void Awake()
    {
        instance = this;
    }

    public event Action onStartGame;
    public void StartGame()
    {
        onStartGame?.Invoke();
    }

    public event Action onSpeedUp;
    public void SpeedUp()
    {
        onSpeedUp?.Invoke();
    }

    public event Action onSpeedDown;
    public void SpeedDown()
    {
        onSpeedDown?.Invoke();
    }

    public event Action onAsteroidPassed;
    public void AsteroidPassed()
    {
        onAsteroidPassed?.Invoke();
    }

    public event Action onGameOver;
    public void GameOver()
    {
        onGameOver?.Invoke();
    }
}