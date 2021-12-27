using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorBase : MonoBehaviour
{
    [Header("Base Properties")]
    [SerializeField]
    protected GameObject[] objectPrefabs;
    [SerializeField]
    protected int maxObjectsAmount = 15;
    [SerializeField]
    protected float destroyZPoint = -10f;

    protected List<GameObject> currentObjects;
    protected float secondsBetweenGeneration;

    // Generate an object with offset
    protected void GenerateObject(Vector3 offset)
    {
        // Events binding
        GameEvents.instance.onSpeedUp += SetTime;
        GameEvents.instance.onSpeedDown += SetTime;
        GameEvents.instance.onStartGame += StartGame;

        // Choose random object prefab
        int randomSegment = Random.Range(0, objectPrefabs.Length);

        // Define starting position of the object
        // depending on the offset from spawner
        Vector3 pos = transform.position;
        pos += new Vector3(offset.x, offset.y, offset.z);

        // Instantiate object and add to the current segments list
        currentObjects.Add(Instantiate(objectPrefabs[randomSegment], pos, Quaternion.identity));
    }

    // Set secondsBetweenGeneration function
    protected virtual void SetTime()
    {
        Debug.Log("Base set time");
    }

    // Reaction to game start
    protected virtual void StartGame()
    {
        Debug.Log("Generation begins");
    }

    // Check if some objects need to be deleted
    protected void CheckDestroyRequirement()
    {
        List<GameObject> objectsToRemove = new List<GameObject>();

        // Check if any objects are out of main gameplay area
        foreach (var obj in currentObjects)
        {
            if (obj.transform.position.z < destroyZPoint)
                objectsToRemove.Add(obj);
        }

        // Destroy these objects
        foreach (var obj in objectsToRemove)
        {
            currentObjects.Remove(obj);
            Destroy(obj);
        }
    }

    // Main generator update function
    protected virtual IEnumerator GeneratorUpdate()
    {
        yield return new WaitForSeconds(0f);
    }
}