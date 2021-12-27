using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPS;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals("Asteroid"))
        {
            GameEvents.instance.GameOver();
            Instantiate(explosionPS, new Vector3(transform.position.x, transform.position.y + 0.85f, transform.position.z), transform.rotation);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Asteroid")
        {
            other.gameObject.GetComponent<Collider>().enabled = false;
            GameEvents.instance.AsteroidPassed();
        }
    }
}
