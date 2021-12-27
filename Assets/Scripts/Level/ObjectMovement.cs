using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    void Update()
    {
        transform.Translate(-Vector3.forward * LevelProperties.instance.LevelMovementSpeed * Time.deltaTime);
    }
}