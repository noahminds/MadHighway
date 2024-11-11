using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;  // Singleton instance
    private AudioSource audioSource;

    void Awake()
    {
        // Ensure only one instance of AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Keeps AudioManager across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate instances
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f; // Set to 2D sound
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
