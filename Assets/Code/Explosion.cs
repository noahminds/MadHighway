using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float duration = 1f;

    void Start()
    {
        // Destroy the explosion GameObject after the specified duration
        Invoke(nameof(Destruct), duration);
    }

    public void Destruct() {
        Destroy(gameObject);
    }
}