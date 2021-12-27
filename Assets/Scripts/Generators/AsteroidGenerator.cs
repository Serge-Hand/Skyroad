using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGenerator : GeneratorBase
{
    [Header("Extra Properties")]
    [SerializeField]
    private float minInbetweenDistance = 10f;
    [SerializeField]
    private float minYPosition = 0.5f;
    [SerializeField]
    private float positionRandomFactor = 5f;
    [SerializeField]
    private float asteroidGenerationZOffset = 20f;
    [SerializeField]
    private float difficultyRiseTime = 10f;
    [SerializeField]
    private float generationDelay = 1f;
    [SerializeField]
    private float generationDelayShredStep = 0.05f;

    private float levelBounds;

    void Start()
    {
        // Get level bounds
        levelBounds = LevelProperties.instance.LevelBoundsX;

        // Generate first asteroid
        currentObjects = new List<GameObject>();
        GenerateObject(new Vector3(Random.Range(-levelBounds, levelBounds), minYPosition, asteroidGenerationZOffset));
    }

    protected override void SetTime()
    {
        // Calculate generation time
        secondsBetweenGeneration = generationDelay * 1f / LevelProperties.instance.LevelMovementSpeed * 10f;
    }

    protected override void StartGame()
    {
        // Start endless asteroids generation
        StartCoroutine(GeneratorUpdate());
    }

    protected override IEnumerator GeneratorUpdate()
    {
        SetTime();
        // Set difficulty rise timer
        StartCoroutine(DifficultyTimer());
        while (true)
        {
            // Generate 1 asteroid every secondsBetweenGeneration seconds
            // if not max amount of asteroids on the scene
            yield return new WaitForSeconds(secondsBetweenGeneration);

            if (currentObjects.Count < maxObjectsAmount)
            {
                // Check if the offset between 2 asteroids is
                // big enough for the spaceship
                if ((asteroidGenerationZOffset - currentObjects[currentObjects.Count - 1].transform.position.z) > minInbetweenDistance)
                {
                    float distance = Random.Range(-positionRandomFactor, positionRandomFactor) + asteroidGenerationZOffset;
                    Vector3 offset = new Vector3(Random.Range(-levelBounds, levelBounds), minYPosition, distance);
                    GenerateObject(offset);
                }
            }

            // Check if some asteroids are ready to be destroyed
            CheckDestroyRequirement();
        }
    }

    private IEnumerator DifficultyTimer()
    {
        while (true)
        {
            // Wait difficultyRiseSeconds seconds then
            // decrease asteroids generation delay
            yield return new WaitForSeconds(difficultyRiseTime);
            if (generationDelay > 0.1f)
            {
                generationDelay -= generationDelayShredStep;
                SetTime();
                Debug.Log("Difficulty rise");
            }
            else
            {
                generationDelay = 0.1f;
                SetTime();
                break;
            }
        }
        Debug.Log("Max difficulty!");
        yield return new WaitForSeconds(0f);
    }
}