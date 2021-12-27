using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public float distance = 10.0f;
    public float speedUpDistance = 8f;
    public float height = 5.0f;
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;
    public float distanceDamping = 0.1f;
    public Transform target;

    private bool isSpeedUp = false;

    private void Start()
    {
        // Events binding
        GameEvents.instance.onSpeedUp += SpeedUp;
        GameEvents.instance.onSpeedDown += SpeedDown;
    }

    private void SpeedUp()
    {
        isSpeedUp = true;
    }

    private void SpeedDown()
    {
        isSpeedUp = false;
    }

    private void LateUpdate()
    {
        // Early out if we don't have a target
        if (!target)
        {
            return;
        }

        // Calculate the current rotation angles
        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Calculate current distance
        float wantedDistance;
        float currentDistance = transform.position.z;
        // Change wanted distance if speeding up or speeding down
        if (isSpeedUp)
        {
            wantedDistance = -speedUpDistance;
        }
        else
        {
            wantedDistance = -distance;
        }
        // Damp the distance
        currentDistance = Mathf.Lerp(currentDistance, wantedDistance, distanceDamping * Time.deltaTime);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        var pos = transform.position;
        pos = target.position - currentRotation * Vector3.forward;
        pos.y = currentHeight;
        pos.z = currentDistance;
        transform.position = pos;

        // Always look at the target
        transform.LookAt(target);
    }
}