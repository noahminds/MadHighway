using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject Prefab;           // Projectile prefab
    public Transform firePoint;         // The position from which the projectiles are fired relative to the turret
    public float shotDelay = 0.2f;      // Delay between each projectile in the burst
    public float projectileSpeed = 10f; // Speed of the projectiles
    private bool isShooting = false;    // To prevent firing multiple shots simultaneously

    // <summary>
    // Fires when the "F" key is pressed
    // </summary>
    void Update()
    {
        if (Input.GetKey(KeyCode.F) && !isShooting)
        {
            StartCoroutine(Fire());
        }
    }

    // <summary>
    // Fires projectiles using coroutine to add delay between each projectile
    // without blocking the main thread
    // </summary>
    private System.Collections.IEnumerator Fire()
    {
        isShooting = true;

        FireProjectile();
        yield return new WaitForSeconds(shotDelay);

        isShooting = false;
    }

    // <summary>
    // Shoot a projectile
    // </summary>
    private void FireProjectile()
    {
        GameObject projectile = Instantiate(Prefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            rb.velocity = transform.up * projectileSpeed;
        }
    }
}
