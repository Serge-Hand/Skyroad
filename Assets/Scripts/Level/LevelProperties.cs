using UnityEngine;

public class LevelProperties : MonoBehaviour
{
    public static LevelProperties instance;

    private float tmpLevelMovementSpeed;

    private void Awake()
    {
        instance = this;

        // Events binding
        GameEvents.instance.onStartGame += StartGame;
        GameEvents.instance.onSpeedUp += SpeedUp;
        GameEvents.instance.onSpeedDown += SpeedDown;
        GameEvents.instance.onGameOver += GameOverSetUp;

        tmpLevelMovementSpeed = levelMovementSpeed;
        levelMovementSpeed = 0f;
    }

    private void StartGame()
    {
        levelMovementSpeed = tmpLevelMovementSpeed;
    }

    private void SpeedUp()
    {
        levelMovementSpeed *= superSpeedMultiplier;
    }

    private void SpeedDown()
    {
        levelMovementSpeed /= superSpeedMultiplier;
    }

    private void GameOverSetUp()
    {
        levelMovementSpeed = 0f;
    }

    [SerializeField]
    private float levelMovementSpeed = 1f; // How fast level is moving
    public float LevelMovementSpeed => levelMovementSpeed;
    [SerializeField]
    private float levelBoundsX = 1f; // Bounds of player's horizontal movement within the level
    public float LevelBoundsX => levelBoundsX;
    [SerializeField]
    private float superSpeedMultiplier = 2f; // Speed up multiplier
}