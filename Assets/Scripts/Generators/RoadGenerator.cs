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
        StartCoroutine(GeneratorUpdate());
    }

    /*protected override void GenerateObjects(int amount, Vector3 offset)
    {
        Vector3 pos = new Vector3(0, 0, 0);

        for (int i = 0; i < amount; i++)
        {
            // Choose random segment prefab
            int randomSegment = Random.Range(0, objectPrefabs.Length);

            if (currentObjects.Count > 0)
            {
                // Define starting position of the segment
                // depending on the position and size of the previous segment
                pos = currentObjects[currentObjects.Count - 1].transform.position;
                pos += new Vector3(0, 0, currentObjects[currentObjects.Count - 1].GetComponent<Renderer>().bounds.size.z);
            }

            // Instantiate segment and add to the current segments list
            currentObjects.Add(Instantiate(objectPrefabs[randomSegment], pos, Quaternion.identity));
        }
    }*/

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