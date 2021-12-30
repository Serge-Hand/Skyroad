using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : GeneratorBase
{
    void Start()
    {
        // Generate inital road segments
        currentObjects = new List<GameObject>();
        for (int i = 0; i < maxObjectsAmount; i++)
        {
            if (currentObjects.Count < 1)
                GenerateObject(Vector3.zero);
            else
            {
                Vector3 offset = new Vector3(0, 0, 
                    currentObjects[currentObjects.Count - 1].GetComponent<Renderer>().bounds.size.z + 
                    currentObjects[currentObjects.Count - 1].transform.position.z);
                GenerateObject(offset);
            }
        }
    }

    protected override void SetTime()
    {
        // Calculate optimal generation time
        secondsBetweenGeneration = 1f / LevelProperties.instance.LevelMovementSpeed * 10f;
    }

    protected override void StartGame()
    {
        // Start endless segments generation
        Debug.Log("Road generation begins");
        StartCoroutine(GeneratorUpdate());
    }

    protected override IEnumerator GeneratorUpdate()
    {
        SetTime();
        while (true)
        {
            // Generate 1 road segment every secondsBetweenGeneration seconds
            // if not max amount of segments on the scene
            if (currentObjects.Count < maxObjectsAmount)
            {
                Vector3 offset = new Vector3(0, 0, 
                    currentObjects[currentObjects.Count - 1].GetComponent<Renderer>().bounds.size.z +
                    currentObjects[currentObjects.Count - 1].transform.position.z);
                GenerateObject(offset);
            }
            // Check if some segments are ready to be destroyed
            CheckDestroyRequirement();

            yield return new WaitForSeconds(secondsBetweenGeneration);
        }
    }
}