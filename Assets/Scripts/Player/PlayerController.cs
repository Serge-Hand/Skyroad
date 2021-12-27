using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float horizontalSpeed = 5f;
    [SerializeField]
    private GameObject[] thrusts;

    private Rigidbody rigidBody;
    private Animator animator;
    private Vector3 horizontalMovement;

    private float levelBounds;

    private bool isGameStarted = false;
    private bool isSpeedUp = false;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        levelBounds = LevelProperties.instance.LevelBoundsX;
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (isGameStarted)
        {
            // Get horizontal input from player
            horizontalMovement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);

            // Inrease speed while holding speed up button
            if (Input.GetButtonDown("Jump") && !isSpeedUp)
            {
                Debug.Log("Super speed activated");
                foreach (GameObject obj in thrusts)
                {
                    // Inrease particle system lenght
                    var main = obj.GetComponent<ParticleSystem>().main;
                    main.startLifetime = 1f;
                }
                GameEvents.instance.SpeedUp();
                isSpeedUp = true;
            }
            if (Input.GetButtonUp("Jump") && isSpeedUp)
            {
                Debug.Log("Super speed deactivated");
                foreach (GameObject obj in thrusts)
                {
                    // Decrease particle system lenght
                    var main = obj.GetComponent<ParticleSystem>().main;
                    main.startLifetime = 0.5f;
                }
                GameEvents.instance.SpeedDown();
                isSpeedUp = false;
            }
        }
        else
        {
            // Wait for any key pressing before game start
            if (Input.anyKey)
            {
                isGameStarted = true;
                animator.SetBool("Start", true);
                foreach(GameObject obj in thrusts)
                {
                    obj.SetActive(true);
                }
                GameEvents.instance.StartGame();
            }
        }
    }

    void FixedUpdate()
    {
        if (isGameStarted)
        {
            // Move horizontally if within level's bounds
            if ((horizontalMovement.x < 0 && transform.position.x > -levelBounds) ||
                (horizontalMovement.x > 0 && transform.position.x < levelBounds))
                rigidBody.MovePosition(transform.position + horizontalMovement * Time.fixedDeltaTime * horizontalSpeed);

            // Tilt spaceship depending on movement direction
            animator.SetFloat("Movement", horizontalMovement.x);
        }
    }
}