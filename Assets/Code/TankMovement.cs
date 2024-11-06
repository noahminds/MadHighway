using UnityEngine;

//
// This class is responsible for the movement of the tank using the arrow buttons
// on the keyboard
//
public class TankMovement : MonoBehaviour
{
    public float speedX = 6f;  // Speed at which tank move side-to-side
    public float speedY = 4f;  // Speed at which tank moves up and down
    private Camera mainCamera; // Store reference to the main camera at start
    private Rigidbody2D rb;    // Store reference to the Rigidbody2D component at start

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from arrow keys
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Move the tank
        Vector3 movement = new Vector3(moveX * speedX, moveY * speedY, 0) * Time.deltaTime;
        transform.Translate(movement);

        // Stop the tank from moving off-screen
        ClampPosition();
    }

    // <summary>
    // On collision with a vehicle, the vehicle explodes
    // </summary>
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Vehicle"))
        {
            Vehicle vehicle = col.gameObject.GetComponent<Vehicle>();
            if (vehicle != null)
            {
                vehicle.Explode(col.gameObject);
                rb.velocity = Vector2.zero;
            }
        }
    }

    // <summary>
    // Clamp the tank's position within the screen boundaries
    // </summary>
    void ClampPosition()
    {
        Vector3 position = transform.position;

        float screenTop = mainCamera.orthographicSize;
        float screenBottom = -screenTop;
        float screenRight = screenTop * mainCamera.aspect;
        float screenLeft = -screenRight;

        position.x = Mathf.Clamp(position.x, screenLeft, screenRight);
        position.y = Mathf.Clamp(position.y, screenBottom, screenTop);

        transform.position = position;
    }
}
