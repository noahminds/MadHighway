using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    public GameObject[] vehiclePrefabs;        // Array of different vehicle prefabs to spawn
    public float spawnInterval = 2f;       // Time interval between vehicle spawns
    public float topSpawnY = 6f;           // Y-position for spawning vehicles at the top of the screen

    private List<float> lanePositions = new List<float> { -3f, -1.5f, 0f, 1.5f, 3f }; // X-positions for each lane

    void Start()
    {
        StartCoroutine(SpawnVehicles());
    }

    // <summary>
    // Coroutine to spawn vehicles at regular intervals
    // </summary>
    IEnumerator SpawnVehicles()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnVehicle();
        }
    }

    // <summary>
    // Spawns a vehicle in a random lane at the top of the screen and temporarily removes the lane to prevent overlapping spawns.
    // </summary>
    void SpawnVehicle()
    {
        // Randomly select a lane for the new vehicle and remove it from the available lanes
        int laneIndex = Random.Range(0, lanePositions.Count);
        float spawnX = lanePositions[laneIndex];

        // Select a random vehicle prefab
        GameObject vehiclePrefab = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)];

        // Instantiate the vehicle at the selected lane position
        GameObject newVehicle = Instantiate(vehiclePrefab, new Vector3(spawnX, topSpawnY, 0), Quaternion.identity);

        // Set the vehicle's downward velocity
        Rigidbody2D rb = newVehicle.GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("Rigidbody2D component not found on vehicle prefab: " + vehiclePrefab.name);

        Vehicle vehicle = newVehicle.GetComponent<Vehicle>();
        if (vehicle == null)
            Debug.LogError("Vehicle component not found on vehicle prefab: " + vehiclePrefab.name);

        float vehicleSpeed = 1 / vehicle.speed;
        rb.velocity = new Vector2(0, -vehicleSpeed);

        // Remove the lane temporarily to prevent overlapping spawns
        lanePositions.RemoveAt(laneIndex);

        // Restore the lane position after a short delay to allow the vehicle to move down
        StartCoroutine(RestoreLane(spawnX));
    }

    // <summary>
    // Coroutine to restore a lane position after a short delay
    // </summary>
    IEnumerator RestoreLane(float lanePosition)
    {
        // Wait for a short delay to allow the vehicle to move down the screen
        yield return new WaitForSeconds(1f);

        lanePositions.Add(lanePosition);
    }
}