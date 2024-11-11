using Unity.VisualScripting;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public float speed = 0.5f;          // Inverse of the speed at which the vehicle moves.
    public GameObject ExplosionPrefab;  // The explosion prefab to instantiate when the vehicle is destroyed
    private float healthPerMass = 0.5f;  // The health of the vehicle for each unit of its mass
    private float vehicleHealth;        // The total health of the vehicle
    private Rigidbody2D rigidBody;      // Store reference to the Rigidbody2D component at start
    private Camera mainCamera;          // Store reference to the main camera at start
    private bool spinout = false;       // Flag to indicate if the vehicle is spinning out
    private bool hasExploded = false;
    private AudioSource audioSource;
    
    void Start()
    {
        gameObject.tag = "Vehicle"; // Set the tag of the vehicle to "Vehicle"
        mainCamera = Camera.main;
        rigidBody = GetComponent<Rigidbody2D>();
        vehicleHealth = rigidBody.mass * healthPerMass;
        audioSource = GetComponent<AudioSource>();
    }

    // <summary>
    // The vehicle explodes when it goes out of the screen
    // </summary>
    void Update()
    {
        // Check if the vehicle is off the top or bottom of the screen
        if (IsOutOfScreen())
        {
            Destruct();
        }

        // Check if the vehicle hit the sides of the screen
        if (HitSides())
        {
            Explode(gameObject);
        }
    }

    // <summary>
    // Check if the vehicle is off the top or bottom of the screen with a buffer of 1 unit
    // </summary>
    bool IsOutOfScreen()
    {
        Vector3 position = transform.position;

        float screenTop = mainCamera.orthographicSize;
        float screenBottom = -screenTop;

        return position.y < screenBottom - 3 || position.y > screenTop + 3;
    }

    // <summary>
    // Check if the vehicle is out of the screen
    // </summary>
    bool HitSides()
    {
        Vector3 position = transform.position;

        float screenTop = mainCamera.orthographicSize;
        float screenRight = screenTop * mainCamera.aspect;
        float screenLeft = -screenRight;

        return position.x < screenLeft || position.x > screenRight;
    }

    // <summary>
    // Destroys the vehicle
    // </summary>
    public void Destruct() {
        Destroy(gameObject);
    }

    // <summary>
    // Explodes the vehicle and increments the score proportional to the vehicle's mass
    // </summary>
    public void Explode(GameObject vehicle)
    {
        if (hasExploded) return; 
        hasExploded = true;

        // Increment the score by the vehicle's mass
        ScoreKeeper.AddToScore(rigidBody.mass);

        // Play the explosion sound through the AudioManager
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySound(audioSource.clip);
        }

        // Turn on CircleCollider2D
        GetComponent<CircleCollider2D>().enabled = true;
        
        // Turn on PointEffector2D and make its force proportional to the vehicle's mass
        PointEffector2D pointEffector = GetComponent<PointEffector2D>();
        pointEffector.enabled = true;
        pointEffector.forceMagnitude = rigidBody.mass * 50;

        // Turn off the SpriteRenderer
        GetComponent<SpriteRenderer>().enabled = false;

        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity, transform.parent);

        // Schedule a destruct to occur in 0.1 seconds using invoke
        Invoke(nameof(Destruct), 0.1f);
    }

    // <summary>
    // Triggered when a vehicle explodes, setting off a chain reaction of 
    // damage to other vehicles within the radius of the explosion
    void OnTriggerEnter2D(Collider2D col)
    {
        // Check if the colliding object is a vehicle
        if (col.gameObject.CompareTag("Vehicle") && col.gameObject != this.gameObject)
        {
            Vehicle otherVehicle = col.GetComponent<Vehicle>();
            if (otherVehicle != null)
            {
                otherVehicle.TakeDamage(rigidBody.mass * healthPerMass);
            }
        }
    }

    // <summary>
    // Spinout the vehicle
    // </summary>
    public void Spinout(GameObject vehicle)
    {
        if (!spinout)
        {
            // Apply a random torque to the vehicle
            float spinDirection = Random.Range(0, 2) > 1 ? 1 : -1;
            rigidBody.AddTorque(Random.Range(75, 125) * spinDirection);

            // Apply a random horizontal force to the vehicle and a random vertical force 
            // in the negative direction
            float forceDirection = Random.Range(0, 2) > 1 ? 1 : -1;
            rigidBody.AddForce(new Vector2(Random.Range(75, 125) * forceDirection, -Random.Range(50, 100)));

            // Set the spinout flag to true
            spinout = true;
        }
        else
        {
            // Exacerbate existing angular velocity (spin) and linear velocity (movement) by 10%
            rigidBody.angularVelocity *= 1.5f; // Increase spin by 50%
            rigidBody.velocity *= 1.2f;        // Increase linear momentum by 20%
        }
    }

    // <summary>
    // Take damage from a projectile and simulate spinout effect.
    // </summary>
    public void TakeDamage(float damage)
    {
        vehicleHealth -= damage;
        if (vehicleHealth <= 0.0f)
        {
            // log the damage
            Explode(gameObject);
        }

        // If the vehicle is struck for the first time trigger a spinout
        Spinout(gameObject);
    }
}
