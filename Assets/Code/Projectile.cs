using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileDamage = 0.5f; // Damage done by the projectile
    private Camera mainCamera; // Store reference to the main camera at start

    void Start()
    {
        mainCamera = Camera.main;
    }
    
    // <summary>
    // Destroys the projectile if it goes off screen
    // </summary>
    void Update()
    {
        float screenTop = mainCamera.orthographicSize;
        if (transform.position.y > screenTop)
        {
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    // <summary>
    // Collision handling with vehicles. Does 0.5 points of damage on impact.
    // </summary>
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Vehicle"))
        {
            Vehicle vehicle = col.gameObject.GetComponent<Vehicle>();

            if (vehicle != null) {
            vehicle.TakeDamage(projectileDamage);
            }

            Destroy(gameObject);
        }
    }
}
